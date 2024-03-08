using Core.Application.Common;
using Core.Domain.Push;
using Core.Domain.Segment;
using Core.Infrastructure.Persistence.Core;
using Core.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection ConfigurePresistence(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISubscriberRepository, SubscriberRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddSingleton<OutboxInterceptor>();

        services.AddDbContext<ApplicationDbContext>((IServiceProvider serviceProvider,
                                                     DbContextOptionsBuilder options) =>
        {
            options.UseSqlServer(serviceProvider.GetRequiredService<IConfiguration>()
                                                .GetConnectionString("Application"), sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                                errorNumbersToAdd: null);

            });
        });
#if DEBUG
        services.AddHostedService<DBInitialzer>();
#endif
        return services;
    }

    public static IHealthChecksBuilder ConfigurePresistenceHealthChecks(this IHealthChecksBuilder builder)
    {
        builder.AddDbContextCheck<ApplicationDbContext>(nameof(ApplicationDbContext), tags: new[] { "ready", "liveness" });

        return builder;
    }
}
