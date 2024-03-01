using Core.Domain.Push;
using Core.Infrastructure.Persistence.Core;


namespace Core.Infrastructure.Persistence.Configurations.Push;
internal sealed class NotificationEventConfig : IEntityTypeConfiguration<NotificationEvent>
{
    public void Configure(EntityTypeBuilder<NotificationEvent> builder)
    {
        builder.ToTable(nameof(NotificationEvent), DbConst.Push, t => t.HasComment(DBResource.NotificationEvent));

        builder.HasKey(x => x.Id).HasName($"PK_{DbConst.Push}_{nameof(NotificationEvent)}");
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedOnUtc)
               .IsRequired();


    }
}
