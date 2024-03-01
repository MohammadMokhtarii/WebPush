using Core.Infrastructure.Persistence.Core;

namespace Core.Infrastructure.Persistence.Configurations;
internal sealed class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(nameof(OutboxMessage), DbConst.Base, t => t.HasComment(DBResource.OutboxMessage));

        builder.HasKey(x => x.Id).HasName($"PK_{DbConst.Base}_{nameof(OutboxMessage)}");
        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Type)
               .IsRequired();

        builder.Property(x => x.Content)
                .HasColumnType("varchar(max)")
                .IsRequired();
    }
}
