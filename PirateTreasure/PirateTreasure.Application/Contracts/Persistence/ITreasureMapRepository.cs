using PirateTreasure.Domain.Entities;

namespace PirateTreasure.Application.Contracts.Persistence
{
    public interface ITreasureMapRepository
    {
        Task<TreasureMap?> GetByIdAsync(Guid id);

        Task<List<TreasureMap>> GetAllAsync();

        Task AddAsync(TreasureMap map);

        Task UpdateAsync(TreasureMap map);

        Task DeleteAsync(Guid id);

        Task SaveChangesAsync();
    }
}