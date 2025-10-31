using SafeYard.Models;
using SafeYard.Models.Common;

namespace SafeYard.Services.Interfaces
{
    public interface IMotoService
    {
        Task<IReadOnlyList<Moto>> GetAllAsync();
        Task<(IReadOnlyList<Moto> Items, int Total)> GetAsync(string? marca, PagingParameters paging);
        Task<Moto?> GetByIdAsync(int id);
        Task<IReadOnlyList<Moto>> GetByAnoAsync(int? minAno);
        Task<Moto> CreateAsync(Moto moto);
        Task<bool> UpdateAsync(Moto moto);
        Task<bool> DeleteAsync(int id);
    }
}