using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PirateTreasure.Domain.Entities;

namespace PirateTreasure.Infrastructure.Persistence.Configurations
{
    public class TreasureCellConfiguration : IEntityTypeConfiguration<TreasureCell>
    {
        public void Configure(EntityTypeBuilder<TreasureCell> builder)
        {
            builder.ToTable("TreasureCell");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Row).IsRequired();
            builder.Property(x => x.Col).IsRequired();
            builder.Property(x => x.ChestValue).IsRequired();
        }
    }
}