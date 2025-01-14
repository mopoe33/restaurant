using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;
using System.ComponentModel.Design;

namespace restaurant.Controllers
{
    [Authorize]
    public class CommandesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public CommandesController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return View(await _context.Commandes.ToArrayAsync());
            else {
                string UserId = _userManager.GetUserId(User);

                var Orders = await _context.Commandes.Where(e => e.ClientId == UserId).ToListAsync();
               
                    return View("Index", Orders);
            }
        }
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Details(int id)
        {
            var commandeDetails = await _context.Commandes
        .Where(c => c.Id == id)
        .Select(c => new
        {
            Commande = c, // Commande details
            User = _context.Users.FirstOrDefault(u => u.Id == c.ClientId), // User details
            LigneCommandes = _context.LigneCommandes
                .Where(l => l.CommandeId == c.Id)
                .Select(l => new
                {
                    LigneCommande = l, // LigneCommande details
                    Plat = _context.Plats.FirstOrDefault(p => p.Id == l.PlatId) // Plat details
                }).ToList()
        })
        .FirstOrDefaultAsync();

            if (commandeDetails == null)
            {
                return NotFound("Commande not found.");
            }

            return View(commandeDetails);
        }


        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                string UserId = _userManager.GetUserId(User);

                var PanierUser = await _context.Paniers.Where(e => e.ClientId == UserId).ToListAsync();
                if (PanierUser != null)
                {
                    Commande c = new Commande();
                   
                    c.ClientId = UserId;
                    c.DateCommande = DateTime.Now;
                    c.Total = PanierUser.Sum(e => e.Total);
                    c.Statut = "en cours";
                    _context.Add(c);
                    await _context.SaveChangesAsync();
                    foreach (var item in PanierUser)
                    {
                        LigneCommande l = new LigneCommande();
                        l.CommandeId = c.Id;
                        l.PlatId = item.PlatId;
                        l.Quantite = item.Quantite;
                        l.Total = item.Total;
                        _context.Add(l);
                    }

                    
                   
                    _context.RemoveRange(PanierUser);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction("Index", "Home");
            }
            else { return Unauthorized("vous ete pas connecte"); }


        }


        
    }
}
