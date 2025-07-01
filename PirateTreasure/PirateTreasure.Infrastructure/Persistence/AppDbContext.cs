using Microsoft.EntityFrameworkCore;
using PirateTreasure.Domain.Entities;

namespace PirateTreasure.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TreasureMap> TreasureMaps => Set<TreasureMap>();
        public DbSet<TreasureCell> TreasureCells => Set<TreasureCell>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply Fluent Configuration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}