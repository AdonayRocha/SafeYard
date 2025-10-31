using SafeYard.Models;
using SafeYard.Models.Common;

namespace SafeYard.Services.Interfaces
{
    public interface IPatioService
    {
        Task<(IReadOnlyList<Patio> Items, int Total)> GetAsync(PagingParameters paging);
        Task<Patio?> GetByIdAsync(int id);
        Task<Patio> CreateAsync(Patio patio);
        Task<bool> UpdateAsync(Patio patio);
        Task<bool> DeleteAsync(int id);
    }
}