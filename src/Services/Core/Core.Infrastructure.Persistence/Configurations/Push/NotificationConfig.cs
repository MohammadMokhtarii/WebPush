using Core.Domain.Push;
using Core.Domain.Segment;
using Core.Infrastructure.Persistence.Core;



namespace Core.Infrastructure.Persistence.Configurations.Push;

internal sealed class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification), DbConst.Push, t => t.HasComment(DBResource.Notification));

        builder.HasKey(x => x.Id).HasName($"PK_{DbConst.Push}_{nameof(Notification)}");
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedOnUtc)
               .IsRequired();

        builder.ComplexProperty(x => x.Body, x =>
        {
            x.Property(x => x.Title).IsRequired().IsUnicode().HasMaxLength(150);
            x.Property(x => x.Message).IsRequired().IsUnicode().HasMaxLength(500);
        });

        builder.HasOne(x => x.Device)
               .WithMany()
               .HasForeignKey(x => x.DeviceId)
               .HasConstraintName($"FK_{DbConst.Segment}_{nameof(Device)}_{DbConst.Push}_{nameof(Notification)}")
               .OnDelete(DeleteBehavior.NoAction);



        builder.HasMany(x => x.NotificationActivities)
               .WithOne()
               .HasForeignKey(x => x.NotificationId)
               .HasConstraintName($"FK_{DbConst.Push}_{nameof(Notification)}_{DbConst.Push}_{nameof(NotificationEvent)}")
               .OnDelete(DeleteBehavior.NoAction);


        builder.HasMany(x => x.NotificationEvents)
               .WithOne()
               .HasForeignKey(x => x.NotificationId)
               .HasConstraintName($"FK_{DbConst.Push}_{nameof(Notification)}_{DbConst.Push}_{nameof(NotificationActivity)}")
               .OnDelete(DeleteBehavior.NoAction);
    }
}
