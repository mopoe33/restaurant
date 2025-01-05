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
    public class ServeursController : Controller
    {
        private readonly AppDbContext _context;

        public ServeursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Serveurs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Serveurs.ToListAsync());
        }

        // GET: Serveurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serveur = await _context.Serveurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serveur == null)
            {
                return NotFound();
            }

            return View(serveur);
        }

        // GET: Serveurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Serveurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,firstName,lastName,timeStart,timeEnd")] Serveur serveur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serveur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serveur);
        }

        // GET: Serveurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serveur = await _context.Serveurs.FindAsync(id);
            if (serveur == null)
            {
                return NotFound();
            }
            return View(serveur);
        }

        // POST: Serveurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,firstName,lastName,timeStart,timeEnd")] Serveur serveur)
        {
            if (id != serveur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serveur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServeurExists(serveur.Id))
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
            return View(serveur);
        }

        // GET: Serveurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serveur = await _context.Serveurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serveur == null)
            {
                return NotFound();
            }

            return View(serveur);
        }

        // POST: Serveurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serveur = await _context.Serveurs.FindAsync(id);
            if (serveur != null)
            {
                _context.Serveurs.Remove(serveur);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServeurExists(int id)
        {
            return _context.Serveurs.Any(e => e.Id == id);
        }
    }
}
