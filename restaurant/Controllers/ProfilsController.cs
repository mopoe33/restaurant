using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<IActionResult> Edit(Users editedUser)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = await _userManager.GetUserIdAsync(user);
            var oldUser = await _context.Users.Where(e => e.Id.Equals(userId)).SingleOrDefaultAsync();

            oldUser.FirstName = editedUser.FirstName;
            oldUser.LastName = editedUser.LastName;
            oldUser.PhoneNumber = editedUser.PhoneNumber;
            oldUser.Email = editedUser.Email;
            oldUser.UserName = editedUser.Email;
            oldUser.NormalizedEmail = editedUser.Email.ToUpper();
            oldUser.NormalizedUserName = editedUser.Email.ToUpper();
            oldUser.City = editedUser.City;
            oldUser.Adresse = editedUser.Adresse;

            
            if (!string.IsNullOrEmpty(editedUser.OldPassword))
            {
                if (string.IsNullOrEmpty(editedUser.NewPassword) || string.IsNullOrEmpty(editedUser.NewConfirmPassword))
                {
                    ViewBag.Message = "Aucun mot de passe n'est spécifié ou vide";
                    return View("Edit");
                }

                if (editedUser.NewPassword != editedUser.NewConfirmPassword)
                {
                    ViewBag.Message = "Les mots de passe ne sont pas conformes";
                    return View("Edit");
                }

                var result = await _userManager.ChangePasswordAsync(user, editedUser.OldPassword, editedUser.NewPassword);
                if (result.Succeeded)
                {
                    _context.Update(oldUser);
                    await _context.SaveChangesAsync();
                    ViewBag.MessageSucc = "Les modifications ont été mises à jour correctement.";
                    return View("Edit");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(oldUser);
            }

            _context.Update(oldUser);
            await _context.SaveChangesAsync();
            ViewBag.MessageSucc = "Les modifications ont été mises à jour correctement.";

            return  View("Edit");
        }

    }
}
