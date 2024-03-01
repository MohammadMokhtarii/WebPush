using Elastic.Apm.NetCoreAll;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Serilog;
using System.IO.Compression;

namespace Services.Common.Presentation;

public static class ApplicationExtentions
{
    private static readonly ushort MericPort = 49402;
    private static readonly int HCPort = 49401;

    public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);
        builder.WebHost.CaptureStartupErrors(true);

        if (builder.Configuration.GetSection("Serilog").Exists())
            builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

        builder.Services.AddDefaultHealthChecks();

        builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
        builder.Services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());

        builder.Services.AddMetricServer(options => options.Port = MericPort);

        return builder;
    }
    public static WebApplication UseServiceDefaults(this WebApplication app)
    {
        app.UseResponseCompression();
        if (!app.Environment.IsDevelopment() && app.Configuration.GetSection("ElasticApm").Exists())
            app.UseAllElasticApm(app.Configuration);

        var pathBase = app.Configuration["PATH_BASE"];
        if (!string.IsNullOrEmpty(pathBase))
            app.UsePathBase(pathBase);

        app.UseServiceMetrics();
        app.MapDefaultHealthChecks();

        return app;
    }

    private static WebApplication UseServiceMetrics(this WebApplication app)
    {
        app.UseHttpMetrics(options =>
        {
            options.ReduceStatusCodeCardinality();
            options.AddCustomLabel("Host_IP", context => context.Request.Host.Host);
        });
        return app;
    }
    private static IHealthChecksBuilder AddDefaultHealthChecks(this IServiceCollection services)
    {
        var hcBuilder = services.AddHealthChecks().ForwardToPrometheus();
        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
        return hcBuilder;
    }
    private static void MapDefaultHealthChecks(this WebApplication routes)
    {
        routes.UseHealthChecks("/hc", HCPort, new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });
        routes.UseHealthChecks("/liveness", HCPort, new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });
        routes.MapMetrics();
    }
}
