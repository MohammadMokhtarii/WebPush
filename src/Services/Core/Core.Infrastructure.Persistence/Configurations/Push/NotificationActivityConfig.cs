using Core.Domain.Push;
using Core.Infrastructure.Persistence.Core;


namespace Core.Infrastructure.Persistence.Configurations.Push;
internal sealed class NotificationActivityConfig : IEntityTypeConfiguration<NotificationActivity>
{
    public void Configure(EntityTypeBuilder<NotificationActivity> builder)
    {
        builder.ToTable(nameof(NotificationActivity), DbConst.Push, t => t.HasComment(DBResource.NotificationActivity));

        builder.HasKey(x => x.Id).HasName($"PK_{DbConst.Push}_{nameof(NotificationActivity)}");
        builder.Property(x => x.Id)
               .HasConversion(valueType => valueType.Value, value => new(value))
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Description)
               .IsUnicode(true)
               .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
               .IsRequired();


    }
}
