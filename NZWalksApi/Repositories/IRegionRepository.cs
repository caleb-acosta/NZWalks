using NZWalksApi.Models.Domain;

namespace NZWalksApi.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> DeleteAsync(Guid id);
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region> CreateAsync(Region region);
    }
}
