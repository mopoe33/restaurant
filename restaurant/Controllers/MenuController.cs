using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;
namespace restaurant.Controllers
{
    public class MenuController : Controller
    {

        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _context.Plats.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plat = await _context.Plats.Include(p => p.PlatIngredients).ThenInclude(pi => pi.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }
    }
}
