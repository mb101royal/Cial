using Cial.Enums;
using Cial.Models;
using Cial.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Cial.Controllers
{
    public class AuthController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly IConfiguration _configuration;

        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // Register

        // Get
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            AppUser user = new()
            {
                Email = vm.Email,
                Fullname = vm.Fullname,
                UserName = vm.Username
            };

            var userCreationResult = await _userManager.CreateAsync(user, vm.Password);

            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
            }

            var currentUser = await _userManager.FindByNameAsync(user.UserName);

            var roleCreationResult = await _userManager.AddToRoleAsync(currentUser, "Member");

            if (!roleCreationResult.Succeeded)
            {
                foreach (var error in roleCreationResult.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        // Login

        // Get
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vm.UsernameOrEmail, vm.Password, false, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Credentials");

            }
            return View(vm);
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        public async Task CreateRoles()
        {
            IdentityResult roleResult;

            var roles = Enum.GetNames(typeof(Roles));

            foreach (string role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);

                if (!roleExist) roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (!await _roleManager.RoleExistsAsync("GAdmin"))
            {
                var role = new IdentityRole();
                role.Name = "GAdmin";
                await _roleManager.CreateAsync(role);
            }

            AppUser gAdminUser = new()
            {
                UserName = _configuration.GetSection("GAdmin")?["UserName"],
                Email = _configuration.GetSection("GAdmin")?["Email"],
                Fullname = _configuration.GetSection("GAdmin")?["Fullname"],
            };

            string userPassword = _configuration.GetSection("GAdmin")?["Password"];

            var _user = await _userManager.FindByEmailAsync(_configuration.GetSection("GAdmin")?["Email"]);

            if (_user == null)
            {
                var createPowerUser = await _userManager.CreateAsync(gAdminUser, userPassword);

                if (createPowerUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(gAdminUser, "GAdmin");
                }
            }
        }
    }
}
