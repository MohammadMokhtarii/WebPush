using Core.Application.Common;
using Core.Infrastructure.BackgroundJobs;
using Core.Infrastructure.Externals;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Core.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
            configure.AddJob<ProcessOutboxMessagesJob>(jobKey)
                     .AddTrigger(trigger => trigger.ForJob(jobKey)
                                                   .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10)
                                                                                           .RepeatForever()));
            var notificationJobKey = new JobKey(nameof(ProcessNotificationJob));
            configure.AddJob<ProcessNotificationJob>(notificationJobKey)
                     .AddTrigger(trigger => trigger.ForJob(notificationJobKey)
                                                   .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(60)
                                                                                           .RepeatForever()));
        }).AddQuartzHostedService();


        services.AddScoped<IWebPushAdapter, WebPushAdapter>();
        return services;
    }

    public static IHealthChecksBuilder ConfigureInfrastructureHealthChecks(this IHealthChecksBuilder builder)
    {

        return builder;
    }
}
