using Core.Domain.Segment;
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


    public async Task TrySeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Seed Database Started");

        await SeedSubscriberAsync(cancellationToken);

        _logger.LogInformation("Seed Database Finished");
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedSubscriberAsync(CancellationToken cancellationToken = default)
    {
        Subscriber subscriber = Subscriber.Create("Default", "http://localhost:3000");
        if (!await _dbContext.Subscribers.AnyAsync(x => x.Name == subscriber.Name && x.Url == subscriber.Url, cancellationToken))
            await _dbContext.Subscribers.AddAsync(subscriber, cancellationToken);
    }
}
