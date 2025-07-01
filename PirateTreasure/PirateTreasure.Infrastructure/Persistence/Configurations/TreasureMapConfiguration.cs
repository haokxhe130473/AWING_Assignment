using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PirateTreasure.Domain.Entities;

namespace PirateTreasure.Infrastructure.Persistence.Configurations
{
    public class TreasureMapConfiguration : IEntityTypeConfiguration<TreasureMap>
    {
        public void Configure(EntityTypeBuilder<TreasureMap> builder)   
        {
            builder.ToTable("TreasureMap");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Rows).IsRequired();
            builder.Property(x => x.Columns).IsRequired();
            builder.Property(x => x.MaxChestValue).IsRequired();

            builder.HasMany(x => x.Cells)
                   .WithOne()
                   .HasForeignKey(x => x.TreasureMapId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}