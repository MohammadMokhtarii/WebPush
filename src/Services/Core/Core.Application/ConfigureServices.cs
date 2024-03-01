using Core.Application.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });
        return services;
    }

}
