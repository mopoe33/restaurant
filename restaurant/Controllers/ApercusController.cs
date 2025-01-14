using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;

namespace restaurant.Controllers
{
    public class ApercusController : Controller
    {
        private readonly AppDbContext _context;

        public ApercusController(AppDbContext context)
        {
            _context = context;
            
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCommandes= await _context.Commandes.Where(e=>e.Statut.Equals("Terminer")).SumAsync(e=>e.Total);
            ViewBag.NumberCommandes = await _context.Commandes.CountAsync();
            ViewBag.NumberReservation = await _context.BookingTables.CountAsync();
            ViewBag.NumberPlat = await _context.Plats.CountAsync();
            return View();
        }
    }
}
