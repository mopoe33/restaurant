using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using restaurant.Models;
using Microsoft.EntityFrameworkCore;
namespace restaurant.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {

        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}
