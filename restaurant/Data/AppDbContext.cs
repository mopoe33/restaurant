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

        public DbSet<Table> Tables { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Plat> Plats { get; set; }

        public DbSet<PlatIngredient>  PlatIngredients { get; set; }

        public DbSet<Panier> Paniers { get; set; }

        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LigneCommandes { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlatIngredient>()
                .HasKey(pi => new { pi.PlatId, pi.IngredientId });

            modelBuilder.Entity<PlatIngredient>()
                .HasOne(pi => pi.Plat)
                .WithMany(p => p.PlatIngredients)
                .HasForeignKey(pi => pi.PlatId);

            modelBuilder.Entity<PlatIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.PlatIngredients)
                .HasForeignKey(pi => pi.IngredientId);
        }


    }
}
