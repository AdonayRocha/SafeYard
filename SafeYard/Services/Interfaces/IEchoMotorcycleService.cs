using SafeYard.Models;

namespace SafeYard.Services.Interfaces
{
    public interface IEchoMotorcycleService
    {
        Task<(IReadOnlyList<EchoMotorcycle> Items, int Total)> GetAsync(int page, int pageSize);
    }
}