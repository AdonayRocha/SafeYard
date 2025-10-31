using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services.Interfaces;

namespace SafeYard.Services
{
    public class MotoService : IMotoService
    {
        private readonly ApplicationDbContext _db;

        public MotoService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<Moto>> GetAllAsync()
        {
            return await _db.Motos
                .AsNoTracking()
                .OrderBy(m => m.Id)
                .ToListAsync();
        }

        public async Task<(IReadOnlyList<Moto> Items, int Total)> GetAsync(string? marca, PagingParameters paging)
        {
            var query = _db.Motos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(marca))
                query = query.Where(m => m.Marca == marca);

            var total = await query.CountAsync();

            IReadOnlyList<Moto> items;
            if (total == 0)
            {
                items = Array.Empty<Moto>();
            }
            else
            {
                items = await query
                    .OrderBy(m => m.Id)
                    .Skip((paging.Page - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToListAsync();
            }

            return (items, total);
        }

        public async Task<Moto?> GetByIdAsync(int id)
        {
            return await _db.Motos
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyList<Moto>> GetByAnoAsync(int? minAno)
        {
            var query = _db.Motos.AsNoTracking().AsQueryable();
            if (minAno.HasValue) query = query.Where(m => m.Ano >= minAno.Value);
            return await query.OrderBy(m => m.Id).ToListAsync();
        }

        public async Task<Moto> CreateAsync(Moto moto)
        {
            _db.Motos.Add(moto);
            await _db.SaveChangesAsync();
            return moto;
        }

        public async Task<bool> UpdateAsync(Moto moto)
        {
            var exists = await _db.Motos.AnyAsync(m => m.Id == moto.Id);
            if (!exists) return false;

            _db.Entry(moto).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Motos.FindAsync(id);
            if (entity == null) return false;

            _db.Motos.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}