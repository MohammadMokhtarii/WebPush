using Core.Domain.Segment;
using Core.Infrastructure.Persistence.Core;



namespace Core.Infrastructure.Persistence.Configurations.Segment;

internal sealed class SubscriberConfig : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable(nameof(Subscriber), DbConst.Segment, t => t.HasComment(DBResource.Subscriber));

        builder.HasKey(x => x.Id).HasName($"PK_{DbConst.Segment}_{nameof(Subscriber)}");
        builder.Property(x => x.Id)
               .HasConversion(valueType => valueType.Value, value => new(value))
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
               .IsRequired()
               .IsUnicode();

        builder.Property(x => x.Url)
               .IsRequired()
               .HasMaxLength(50)
               .IsUnicode(false)
               .HasConversion(x => x.Url, y => WebsiteUrl.Create(y).Data);


        builder.HasMany(x => x.Devices)
               .WithOne()
               .HasForeignKey(x => x.SubscriberId)
               .HasConstraintName($"FK_{DbConst.Segment}_{nameof(Subscriber)}_{DbConst.Segment}_{nameof(Device)}")
               .OnDelete(DeleteBehavior.NoAction);
    }
}
