using CoursesStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoursesStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser { Name = model.Name, UserName = model.Name, Password = model.Password, EmailConfirmed = true};

                var result = await _userManager.CreateAsync(appUser, model.Password);
                await _userManager.AddToRoleAsync(appUser, "User");

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(appUser, false);
                    return RedirectToAction("Index", "Courses");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong password or login.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Courses");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong password or login.");
                }
            }
            return View(model);
        }
    }
}
