using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WanderQuest.Infrastructure.Models;
using WanderQuest.ViewModels.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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

        [Route(nameof(Login))]
        public IActionResult Login()
        {
            return View();
        }

        [Route(nameof(Login))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            AppUser user = await _userManager.FindByEmailAsync(login.Email);

            if (user is null)
            {
                ModelState.AddModelError("", "User does not exist");
                return View(user);
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your user is locked out.\nPlease try later");
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Password", "Password is wrong!");
                return View();
            }

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: "Home");
        }
    }
}
