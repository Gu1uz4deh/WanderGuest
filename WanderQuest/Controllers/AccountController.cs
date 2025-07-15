using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WanderQuest.Infrastructure.Models;
using WanderQuest.Shared.Helpers;
using WanderQuest.ViewModels.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WanderQuest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            await _userManager.AddToRoleAsync(newUser, Enums.Roles.Member.ToString());


            string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            string route = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, token }, HttpContext.Request.Scheme);

            //return RedirectToAction(actionName: "Test", controllerName: "Account", new {route});
            return View("Test", route);

            //return RedirectToAction(actionName: nameof(Index), controllerName: "Home");
        }

        public IActionResult Test(string route)
        {
            return View(route);
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

            bool passwordIsValid = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!passwordIsValid)
            {
                ModelState.AddModelError("Password", "Password is wrong!");
                return View();
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Your mail is not confirmed.\nPlease confirm your email");
                return View();
            }

            bool isLocked = await _userManager.IsLockedOutAsync(user);

            if (isLocked)
            {
                ModelState.AddModelError("", "Your user is locked out.\nPlease try later");
            }



            SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true);

            //if(await _userManager.IsInRoleAsync(user, Enums.Roles.Admin.ToString()))
            //{ 
            //}

            if(User.IsInRole(Enums.Roles.Admin.ToString()))
            {
                return RedirectToAction(actionName: "Index", controllerName: "Dashboard", new {area = "Admin"});
            }

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: "Home");
        }

        public async Task CreateRoles()
        {
            foreach (string role in Enum.GetNames(typeof(Enums.Roles)))
            {
                if(!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return NotFound();
            }
            
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.ConfirmEmailAsync(user, token);
            
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

    }
}
