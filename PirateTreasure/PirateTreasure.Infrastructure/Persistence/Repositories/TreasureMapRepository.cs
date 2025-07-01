using Microsoft.EntityFrameworkCore;
using PirateTreasure.Application.Contracts.Persistence;
using PirateTreasure.Domain.Entities;

namespace PirateTreasure.Infrastructure.Persistence.Repositories
{
    public class TreasureMapRepository : ITreasureMapRepository
    {
        private readonly AppDbContext _context;

        public TreasureMapRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TreasureMap?> GetByIdAsync(Guid id)
        {
            return await _context.TreasureMaps
                                 .Include(m => m.Cells)
                                 .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<TreasureMap>> GetAllAsync()
        {
            return await _context.TreasureMaps
                                 .Include(m => m.Cells)
                                 .ToListAsync();
        }

        public async Task AddAsync(TreasureMap map)
        {
            await _context.TreasureMaps.AddAsync(map);
        }

        public Task UpdateAsync(TreasureMap map)
        {
            _context.TreasureMaps.Update(map);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.TreasureMaps.FindAsync(id);
            if (entity is not null)
                _context.TreasureMaps.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}