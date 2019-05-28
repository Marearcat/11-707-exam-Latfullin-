using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam2.Models;
using Microsoft.AspNetCore.Identity;
using Exam2.Data;
using Exam2.Services;

namespace Exam2.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<IdentityUser> userManager;
        readonly SignInManager<IdentityUser> signInManager;
        readonly RoleManager<IdentityRole> roleManager;
        readonly ApplicationDbContext context;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ChangeRole()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (roleManager.Roles.Any(x => x.Name == "admin"))
                await roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            if (roleManager.Roles.Any(x => x.Name == "user"))
                await roleManager.CreateAsync(new IdentityRole { Name = "user" });
            if(User.IsInRole("admin"))
            {
                await userManager.RemoveFromRoleAsync(user, "admin");
                await userManager.AddToRoleAsync(user, "user");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, "user");
                await userManager.AddToRoleAsync(user, "admin");
            }
            var cartCleaner = new CartService(context);
            cartCleaner.Clean(user.Id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(ViewModels.Registration model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { Email = model.Email, UserName = model.Email };
                // добавляем пользователя
                var result = await userManager.CreateAsync(user, model.Password);
                //if (roleManager.Roles.Any(x => x.Name == "user"))
                    await roleManager.CreateAsync(new IdentityRole { Name = "user" });
                await userManager.AddToRoleAsync(user, "user");
                if (result.Succeeded)
                {
                    // установка куки
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

    }
}
