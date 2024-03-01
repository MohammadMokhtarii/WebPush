using Core.Domain.Segment;
using Core.Infrastructure.Persistence.Core;



namespace Core.Infrastructure.Persistence.Configurations.Segment;

internal sealed class DeviceConfig : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable(nameof(Device), DbConst.Segment, t => t.HasComment(DBResource.Device));

        builder.HasKey(x => x.Id).HasName($"PK_{DbConst.Segment}_{nameof(Device)}");
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
               .IsRequired()
               .IsUnicode();

        builder.ComplexProperty(x => x.PushManager, x =>
        {

        });
        builder.ComplexProperty(x => x.ClientMetadata, x =>
        {
        });



    }
}
