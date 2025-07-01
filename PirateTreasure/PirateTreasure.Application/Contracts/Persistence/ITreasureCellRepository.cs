using PirateTreasure.Domain.Entities;

namespace PirateTreasure.Application.Contracts.Persistence
{
    public interface ITreasureCellRepository
    {
        Task<List<TreasureCell>> GetByMapIdAsync(Guid treasureMapId);

        Task AddRangeAsync(IEnumerable<TreasureCell> cells);

        Task DeleteByMapIdAsync(Guid treasureMapId);

        Task SaveChangesAsync();
    }
}