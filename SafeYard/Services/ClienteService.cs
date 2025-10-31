using Microsoft.EntityFrameworkCore;
using SafeYard.Data;
using SafeYard.Models;
using SafeYard.Models.Common;
using SafeYard.Services.Interfaces;

namespace SafeYard.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _db;

        public ClienteService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(IReadOnlyList<Cliente> Items, int Total)> GetAsync(PagingParameters paging)
        {
            var query = _db.Clientes.AsNoTracking().OrderBy(c => c.Id);

            var total = await query.CountAsync();

            IReadOnlyList<Cliente> items;
            if (total == 0)
            {
                items = Array.Empty<Cliente>();
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

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            _db.Clientes.Add(cliente);
            await _db.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            var exists = await _db.Clientes.AnyAsync(c => c.Id == cliente.Id);
            if (!exists) return false;

            _db.Entry(cliente).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Clientes.FindAsync(id);
            if (entity == null) return false;

            _db.Clientes.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}