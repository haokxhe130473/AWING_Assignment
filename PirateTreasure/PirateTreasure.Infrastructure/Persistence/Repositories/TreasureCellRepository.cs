using Microsoft.EntityFrameworkCore;
using PirateTreasure.Application.Contracts.Persistence;
using PirateTreasure.Domain.Entities;

namespace PirateTreasure.Infrastructure.Persistence.Repositories
{
    public class TreasureCellRepository : ITreasureCellRepository
    {
        private readonly AppDbContext _context;

        public TreasureCellRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TreasureCell>> GetByMapIdAsync(Guid treasureMapId)
        {
            return await _context.TreasureCells
                .Where(c => c.TreasureMapId == treasureMapId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TreasureCell> cells)
        {
            await _context.TreasureCells.AddRangeAsync(cells);
        }

        public async Task DeleteByMapIdAsync(Guid treasureMapId)
        {
            var cells = await _context.TreasureCells
                .Where(c => c.TreasureMapId == treasureMapId)
                .ToListAsync();

            _context.TreasureCells.RemoveRange(cells);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}