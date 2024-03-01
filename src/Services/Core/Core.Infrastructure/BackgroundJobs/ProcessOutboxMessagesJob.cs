using Core.Domain;
using Core.Infrastructure.Persistence;
using Core.Infrastructure.Persistence.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System.Diagnostics;

namespace Core.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublisher publisher, ILogger<ProcessOutboxMessagesJob> logger) : IJob
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IPublisher _publisher = publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger = logger;


    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Exceuting ProcessOutboxMessagesJob Started");


        Stopwatch timer = new();
        timer.Start();

        int pageSize = 100;
        int pageNumber = 1;
        List<OutboxMessage> messages = default!;

        do
        {
            messages = await _dbContext.Set<OutboxMessage>()
                                       .Where(x => x.ProcessedOnUtc == null)
                                       .Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync(context.CancellationToken);

            foreach (var message in messages)
            {
                try
                {
                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                        message.Content,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                    if (domainEvent is null)
                    {
                        _logger.LogWarning("Domain Event Of Type {EventType} Can Not Be Deserialize With id {id} ", message.Type, message.Id);
                        continue;

                    }
                    await _publisher.Publish(domainEvent, context.CancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError("Domain Event Of Type {EventType} Error {@Exception}  With id {id} ", message.Type, e, message.Id);
                    message.Error = e.Message;
                }
                finally
                {
                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
            }
            if (messages.Count != 0)
            {
                await _dbContext.SaveChangesAsync(context.CancellationToken);
                pageNumber++;
            }

        } while (messages.Count != 0);
        timer.Stop();

        _logger.LogInformation("ProcessOutboxMessagesJob Processed {TotalItems} Messages in {TotalSeconds} Seconds", pageNumber * pageSize, timer.Elapsed.TotalSeconds);
    }
}
