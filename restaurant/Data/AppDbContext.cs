using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using restaurant.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.Server;
namespace restaurant.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Serveur> Serveurs { get; set; }
    }
}
