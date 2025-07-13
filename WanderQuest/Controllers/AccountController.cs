using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WanderQuest.Infrastructure.Models;
using WanderQuest.ViewModel;

namespace WanderQuest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Route(nameof(Register))]
        public IActionResult Register()
        {
            return View();
        }

        [Route(nameof(Register))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            AppUser newUser = new AppUser()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.UserName,
                Email = register.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }

            await _signInManager.SignInAsync(newUser, false);

            return RedirectToAction(actionName: nameof(Index), controllerName: "Home");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: "Home");
        }
    }
}
