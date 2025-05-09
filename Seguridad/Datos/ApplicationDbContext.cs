using Microsoft.EntityFrameworkCore;
using Seguridad.Models;


namespace Seguridad.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Log> Logs { get; set; }
    }
}
