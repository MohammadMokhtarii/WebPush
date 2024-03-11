using Core.Domain.Push;
using Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;

namespace Core.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessNotificationJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ProcessNotificationJob> _logger;
    private readonly IConfiguration _configuration;


    public ProcessNotificationJob(ApplicationDbContext dbContext, ILogger<ProcessNotificationJob> logger, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;

    }
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Executing Notifications Job Started");


        Stopwatch timer = new();
        timer.Start();

        int pageSize = 100;
        int pageNumber = 1;
        List<Notification> messages = default!;

        ConnectionFactory factory = CreateConnectionFactory();
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "Push", durable: false, exclusive: false, autoDelete: false, arguments: null);

        do
        {
            messages = await _dbContext.Set<Notification>()
                                       .Include(s => s.Device)
                                       .Where(x => x.NotificationStatusId == NotificationStatus.Pending)
                                       .Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync(context.CancellationToken);


            foreach (var message in messages)
            {
                try
                {
                    _logger.LogInformation(message: "Publishing 1 Messages To RabbitMq Started");

                    var json = JsonConvert.SerializeObject(new
                    {
                        Id = message.Id.Value,
                        Device = message.Device.PushManager,
                        Body = message.Body,
                    });
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish("", "Push", body: body);

                    _logger.LogInformation(message: "Publishing 1 Messages To RabbitMq Ended");


                    message.ChangeStatus(NotificationStatus.Sent, "Change to Sent");

                }
                catch (Exception e)
                {
                    _logger.LogError("Notification Error {@Exception}  With id {id} ", e, message.Id);
                }

            }

            if (messages.Count != 0)
            {
                await _dbContext.SaveChangesAsync(context.CancellationToken);
                pageNumber++;
            }


        } while (messages.Count != 0);

        channel.Close();
        connection.Close();
        timer.Stop();

        _logger.LogInformation("Notifications Processed {TotalItems} Messages in {TotalSeconds} Seconds", pageNumber * pageSize, timer.Elapsed.TotalSeconds);
    }

    private ConnectionFactory CreateConnectionFactory()
    {
        string? connectionString = _configuration.GetConnectionString("MessageQueue");
        ArgumentNullException.ThrowIfNull(connectionString);

        ConnectionFactory factory = new() { Uri = new(connectionString) };
        return factory;
    }
}
