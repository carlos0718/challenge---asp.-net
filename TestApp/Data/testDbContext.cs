using Microsoft.EntityFrameworkCore;
using TestApp.Models;

namespace TestApp.Data {
    public class testDbContext : DbContext {
        public testDbContext(DbContextOptions<testDbContext> options) : base(options) { }
        public DbSet<Suscriptor> Suscriptor { get; set; }
        public DbSet<Suscripcion> Suscripcion { get; set; }
    }
}
