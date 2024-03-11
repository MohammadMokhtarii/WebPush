using Core.Domain.Push;
using Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;

namespace Core.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessNotificationJob(ApplicationDbContext dbContext, ILogger<ProcessNotificationJob> logger) : IJob
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<ProcessNotificationJob> _logger = logger;


    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Executing Notifications Job Started");


        Stopwatch timer = new();
        timer.Start();

        int pageSize = 100;
        int pageNumber = 1;
        List<Notification> messages = default!;

        do
        {
            messages = await _dbContext.Set<Notification>()
                                       .Where(x => x.NotificationStatusId == NotificationStatus.Pending)
                                       .Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync(context.CancellationToken);

            foreach (var message in messages)
            {
                try
                {
                    _logger.LogInformation(message: "Publishing 1 Messages To RabbitMq Started");
                    var factory = new ConnectionFactory { HostName = "rabbitmq" };
                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();
                    channel.QueueDeclare(queue: "Push", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish("", "Push", body: body);
                    _logger.LogInformation(message: "Publishing 1 Messages To RabbitMq Ended");
                    channel.Close();
                    connection.Close();
                    message.NotificationStatusId = NotificationStatus.Sent;

                }
                catch (Exception e)
                {
                    message.NotificationStatusId = NotificationStatus.Failed;
                }

            }

            if (messages.Count != 0)
            {
                await _dbContext.SaveChangesAsync(context.CancellationToken);
                pageNumber++;
            }


        } while (messages.Count != 0);
        timer.Stop();

        _logger.LogInformation("Notifications Processed {TotalItems} Messages in {TotalSeconds} Seconds", pageNumber * pageSize, timer.Elapsed.TotalSeconds);
    }
}
