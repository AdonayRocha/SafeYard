using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;
using SafeYard.Services.Interfaces;

namespace SafeYard.Services
{
    public class EchoMotorcycleService : IEchoMotorcycleService
    {
        private readonly ApplicationDbContext _db;

        public EchoMotorcycleService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(IReadOnlyList<EchoMotorcycle> Items, int Total)> GetAsync(int page, int pageSize)
        {
            var query = _db.TB_ECHO_MOTORCYCLE.AsNoTracking();

            var total = await query.CountAsync();
            if (total == 0)
                return (Array.Empty<EchoMotorcycle>(), 0);

            var items = await query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}