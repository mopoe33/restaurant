using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;

namespace restaurant.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlatsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public PlatsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Plats
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Plats.ToListAsync());
        }

        // GET: Plats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var plat = await _context.Plats.Include(p => p.PlatIngredients).ThenInclude(pi=> pi.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }

        // GET: Plats/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Ingredients"] = new SelectList(await _context.Ingredients.ToListAsync(), "Id", "Name");
            var plat = new Plat();

            return View(plat);
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,IngredientIds")] Plat plat, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;

                if (file != null && file.Length > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    imagePath = Path.Combine("images", fileName);
                    string fullPath = Path.Combine(_imageDirectory, fileName);

                    if (!Directory.Exists(_imageDirectory))
                    {
                        Directory.CreateDirectory(_imageDirectory);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    plat.ImagePath = imagePath;
                }
                _context.Add(plat);
                await _context.SaveChangesAsync();  
                foreach (var ingredientId in plat.IngredientIds)
                {
                    var platIngredient = new PlatIngredient
                    {
                        PlatId = plat.Id,  
                        IngredientId = ingredientId,
                        Quantity = 1
                    };
                    _context.PlatIngredients.Add(platIngredient);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Ingredients"] = new SelectList(await _context.Ingredients.ToListAsync(), "Id", "Name");
            return View(plat);
        }


        // GET: Plats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Ingredients"] = new SelectList(await _context.Ingredients.ToListAsync(), "Id", "Name");
            var plat = await _context.Plats.FindAsync(id);
            if (plat == null)
            {
                return NotFound();
            }
            return View(plat);
        }

        // POST: Plats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,IngredientIds")] Plat plat, IFormFile? file)
        {
            if (id != plat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imagePath = null;

                    if(file != null && file.Length > 0){
                        string fileName = Path.GetFileName(file.FileName);
                         imagePath = Path.Combine("images", fileName);
                        string fullPath = Path.Combine(_imageDirectory, fileName);

                        if (!Directory.Exists(_imageDirectory))
                        {
                            Directory.CreateDirectory(_imageDirectory);
                        }

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        plat.ImagePath = imagePath;
                    }
            else{
                        plat.ImagePath = _context.Plats.AsNoTracking().FirstOrDefault(p => p.Id == plat.Id)?.ImagePath;
                    }

                    _context.Update(plat);
                    var existingIngredients = _context.PlatIngredients.Where(pi => pi.PlatId == plat.Id).ToList();
                    _context.PlatIngredients.RemoveRange(existingIngredients);
                    foreach (var ingredientId in plat.IngredientIds)
                    {
                        var platIngredient = new PlatIngredient
                        {
                            PlatId = plat.Id,
                            IngredientId = ingredientId,
                            Quantity = 1
                        };
                        _context.PlatIngredients.Add(platIngredient);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatExists(plat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); 
                }
            }
            return View(plat);
        }

        // GET: Plats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plat = await _context.Plats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }

        // POST: Plats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plat = await _context.Plats.FindAsync(id);
            if (plat != null)
            {
                _context.Plats.Remove(plat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatExists(int id)
        {
            return _context.Plats.Any(e => e.Id == id);
        }
    }
}
