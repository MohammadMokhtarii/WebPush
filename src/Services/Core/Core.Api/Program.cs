using Asp.Versioning;
using Core.Api.Core;
using Core.Api.GRPC;
using Core.Application;
using Core.Infrastructure;
using Core.Infrastructure.Persistence;
using Services.Common.Presentation;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


builder.Services.ConfigureApplication()
                .ConfigurePresistence()
                .ConfigureInfrastructure();

builder.Services.AddHealthChecks()
                .ConfigurePresistenceHealthChecks();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.Interceptors.Add<GrpcGlobalExceptionHandlerInterceptor>();
});//.AddJsonTranscoding();

#endregion

#region Application

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseServiceDefaults();

app.UseExceptionHandler();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<SubscriberGrpcService>();

app.Run();
#endregion