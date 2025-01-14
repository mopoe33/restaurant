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




        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }
            return View(commande);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Statut")] Commande commande)
        {
            if (id != commande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var existingCommande = await _context.Commandes
                        .Include(c => c.LignesCommande) 
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (existingCommande == null)
                    {
                        return NotFound();
                    }

                    
                    if (commande.Statut == "Terminer" && existingCommande.Statut != "Terminer")
                    {
                        foreach (var ligneCommande in existingCommande.LignesCommande)
                        {
                            
                            var plat = await _context.Plats
                                .Include(p => p.PlatIngredients) 
                                .ThenInclude(pi => pi.Ingredient) 
                                .FirstOrDefaultAsync(p => p.Id == ligneCommande.PlatId);

                            if (plat != null && plat.PlatIngredients != null)
                            {
                                foreach (var platIngredient in plat.PlatIngredients)
                                {
                                    
                                    var ingredient = platIngredient.Ingredient;
                                    if (ingredient != null)
                                    {
                                        ingredient.Quantity -= platIngredient.Quantity * ligneCommande.Quantite;

                                        
                                        if (ingredient.Quantity < 0)
                                        {
                                            ingredient.Quantity = 0;
                                        }

                                        
                                        _context.Update(ingredient);
                                    }
                                }
                            }
                        }
                    }

                    // Update the Commande Statut
                    existingCommande.Statut = commande.Statut;

                    // Save changes to the database
                    _context.Update(existingCommande);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(commande);
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
