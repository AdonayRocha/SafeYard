using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services.Interfaces;

namespace SafeYard.Services
{
    public class PatioService : IPatioService
    {
        private readonly ApplicationDbContext _db;

        public PatioService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(IReadOnlyList<Patio> Items, int Total)> GetAsync(PagingParameters paging)
        {
            var query = _db.Patios.AsNoTracking().OrderBy(p => p.Id);

            var total = await query.CountAsync();

            IReadOnlyList<Patio> items;
            if (total == 0)
            {
                items = Array.Empty<Patio>();
            }
            else
            {
                items = await query
                    .Skip((paging.Page - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToListAsync();
            }

            return (items, total);
        }

        public async Task<Patio?> GetByIdAsync(int id)
        {
            return await _db.Patios.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patio> CreateAsync(Patio patio)
        {
            _db.Patios.Add(patio);
            await _db.SaveChangesAsync();
            return patio;
        }

        public async Task<bool> UpdateAsync(Patio patio)
        {
            var exists = await _db.Patios.AnyAsync(p => p.Id == patio.Id);
            if (!exists) return false;

            _db.Entry(patio).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Patios.FindAsync(id);
            if (entity == null) return false;

            _db.Patios.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}