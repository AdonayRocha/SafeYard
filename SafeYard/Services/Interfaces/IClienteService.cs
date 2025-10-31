using SafeYard.Models;
using SafeYard.Models.Common;

namespace SafeYard.Services.Interfaces
{
    public interface IClienteService
    {
        Task<(IReadOnlyList<Cliente> Items, int Total)> GetAsync(PagingParameters paging);
        Task<Cliente?> GetByIdAsync(int id);
        Task<Cliente> CreateAsync(Cliente cliente);
        Task<bool> UpdateAsync(Cliente cliente);
        Task<bool> DeleteAsync(int id);
    }
}