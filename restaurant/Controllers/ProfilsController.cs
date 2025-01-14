using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;

namespace restaurant.Controllers
{
    [Authorize]
    public class ProfilsController : Controller
    {

        private readonly AppDbContext _context;

        private readonly UserManager<Users> _userManager;

        public ProfilsController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = await _userManager.GetUserIdAsync(user);

            return View(await _context.Users.Where(e=> e.Id.Equals(userId)).SingleOrDefaultAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Users editedUder)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = await _userManager.GetUserIdAsync(user);
            var oldUser = await _context.Users.Where(e => e.Id.Equals(userId)).SingleOrDefaultAsync();
            oldUser.FirstName=editedUder.FirstName;
            oldUser.LastName=editedUder.LastName;
            oldUser.PhoneNumber=editedUder.PhoneNumber;
            oldUser.Email=editedUder.Email;
            oldUser.UserName = editedUder.Email;
            oldUser.NormalizedEmail = editedUder.Email.ToUpper();
            oldUser.NormalizedUserName = editedUder.Email.ToUpper();
            oldUser.City=editedUder.City;
            oldUser.Adresse=editedUder.Adresse;
            _context.Update(oldUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Profils");

        }
    }
}
