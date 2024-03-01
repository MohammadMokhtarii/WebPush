using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Persistence.Core;

public class DBInitialzer : BackgroundService
{
    private readonly ILogger<DBInitialzer> _logger;
    private readonly ApplicationDbContext _dbContext;
    public DBInitialzer(IServiceScopeFactory serviceScopeFactory)
    {
        var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<DBInitialzer>>();
        _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Initializing Database Started");
        try
        {
            await _dbContext.Database.MigrateAsync(stoppingToken);
            await TrySeedAsync(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogInformation("Initializing Database Encountered Exception - {message}({@exception})", e.Message, e);
        }
        _logger.LogInformation("Initializing Database Finished");
    }


    public Task TrySeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Seed Database Started");


        _logger.LogInformation("Seed Database Finished");

        return Task.CompletedTask;
    }
}
