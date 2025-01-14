using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;


namespace restaurant.Controllers
{
    [Authorize(Roles = "Client")]
    public class PanierController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public PanierController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string UserId = _userManager.GetUserId(User);
                var platsInPanier = await _context.Paniers.Where(p => p.ClientId == UserId).Join(_context.Plats,panier => panier.PlatId,plat => plat.Id,(panier, plat) => new{
                    PlatId = plat.Id,
                    PlatName = plat.Name,
                    PlatDescription = plat.Description,
                    PlatPrice = plat.Price,
                    Quantity = panier.Quantite,
                    panier.Total,
                    plat.ImagePath,
                    panier.Id,

                }
            ).ToListAsync();
                return View(platsInPanier);

            }
            return Unauthorized("vous etes pas connecte");
        }

        public async Task<IActionResult> Add(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string UserId = _userManager.GetUserId(User);

                var existingPanier = await _context.Paniers
            .FirstOrDefaultAsync(e => e.ClientId == UserId && e.PlatId == Id);
                if (existingPanier != null)
                {
                   var price = await _context.Plats.Where(e => e.Id == Id).Select(e => e.Price).SingleOrDefaultAsync();
                    existingPanier.Quantite += 1;
                    existingPanier.Total = existingPanier.Quantite * price;
                    _context.Update(existingPanier);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    var platPrice = await _context.Plats.Where(e => e.Id == Id).Select(e => e.Price).SingleOrDefaultAsync();
                    if (platPrice == 0)
                    {
                        return NotFound("Plat n'existe pas.");
                    }
                    Panier p = new Panier();
                    p.ClientId = UserId;
                    p.PlatId = Id;
                    p.Quantite = 1;
                    p.Total = platPrice;

                    _context.Add(p);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Panier");
            }
            else
            {
                return Unauthorized("vous ete pas connecter");
            }
        }




        public async Task<IActionResult> updateQuantite(int id)
        {
            int newQuantite = int.Parse(Request.Query["Quantite"]);

            var p = await _context.Paniers.Where(e => e.Id == id).SingleOrDefaultAsync();
            var pricePlat = await _context.Plats.Where(e => e.Id == p.PlatId).Select(e => e.Price).SingleOrDefaultAsync();
            p.Quantite = newQuantite;
            p.Total = pricePlat * newQuantite;

            _context.Update(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Panier");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Paniers.Where(e => e.Id == id).SingleOrDefaultAsync();
            _context.Remove(p);
             await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Panier");

        }
    }
}
