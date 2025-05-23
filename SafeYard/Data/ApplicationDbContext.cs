using Microsoft.EntityFrameworkCore;
using SafeYard.Models;

namespace SafeYard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

    }
}
