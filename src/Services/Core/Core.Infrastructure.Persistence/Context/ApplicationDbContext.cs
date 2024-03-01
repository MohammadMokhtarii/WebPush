using Core.Domain.Push;
using Core.Domain.Segment;
using Core.Infrastructure.Persistence.Core;
using System.Reflection;

namespace Core.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationActivity> NotificationActivities { get; set; }
    public DbSet<NotificationEvent> NotificationEvents { get; set; }


    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>()
                            .AreUnicode(false)
                            .HaveMaxLength(50);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
