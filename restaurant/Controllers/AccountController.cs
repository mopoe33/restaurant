﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using restaurant.Models;
using restaurant.ViewModels;
using System.Runtime.InteropServices;

namespace restaurant.Controllers
{
    public class AccountController : Controller
    {

        private SignInManager<Users> signInManager;
        private UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,model.RememberMe,false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                {
                    ModelState.AddModelError("", "email ou mot de passe incorrect");
                    return View(model);
                }
            }
            return View(model);
        }




        public async Task<IActionResult> Logout()
        {
           
            await signInManager.SignOutAsync();

            

           
            return RedirectToAction("Index", "Home");
        }
















        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users users = new Users
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    City = model.City,
                    Adresse = model.Adresse
                };

                var result = await userManager.CreateAsync(users, model.Password);
                if (result.Succeeded)
                {
                    var roleExist = await roleManager.RoleExistsAsync("Client");

                    if (!roleExist)
                    {
                        var role = new IdentityRole("Client");
                        await roleManager.CreateAsync(role);
                    }

                    await userManager.AddToRoleAsync(users, "Client");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }
                
            }
            return View(model);
        }


    }
}
