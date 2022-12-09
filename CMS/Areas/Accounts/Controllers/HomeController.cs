using CMS.Data;
using CMS.Models;
using CMS.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        [Route("[area]/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("[area]/login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email");
                return View(model);
            }
            var res = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if (res.Succeeded)
            {
                var admin = await userManager.IsInRoleAsync(user, "Admin");
                var driver = await userManager.IsInRoleAsync(user, "Driver");
                if (admin)
                {
                    return RedirectToAction(nameof(Index), "Home", new { Area = "Admin" });
                }
                else if(driver)
                {
                    return RedirectToAction(nameof(Index), "Driver", new { Area = "Driver" });
                }
                else
                {
                    return RedirectToAction(nameof(Index), "User", new { Area = "User" });
                }

                //return Redirect("/");
            }
            ModelState.AddModelError("", "Invalid Email/Password");
            return View(model);
        }

        [Route("[area]/logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = Guid.NewGuid().ToString().Replace("-", "")
            };
            var res = await userManager.CreateAsync(user, model.Password); ;
            if (res.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                return RedirectToAction(nameof(Login));
            }

            //ModelState.AddModelError("", "An Error Occured!!");
            return View(model);
        }


        [HttpGet]
        public IActionResult RegisterDriver()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterDriver(RegisterDriverViewModel model)
        {
            var userid = "";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = Guid.NewGuid().ToString().Replace("-", "")
            };
            var res = await userManager.CreateAsync(user, model.Password); ;
            if (res.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Driver");
                userid = user.Id;
            }

            db.ApplicationDrivers.Add(new ApplicationDriver()
            {
                LicenceNumber = model.LicenceNumber,
                ValidTo = model.ValidTo,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                UserId = userid
            });

            await db.SaveChangesAsync();

            //ModelState.AddModelError("", "An Error Occured!!");
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> GenerateData()
        {
            await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            await roleManager.CreateAsync(new IdentityRole() { Name = "Driver" });
            await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

            var users = await userManager.GetUsersInRoleAsync("Admin");
            if (users.Count == 0)
            {
                var appUser = new ApplicationUser()
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@gmail.com",
                    UserName = "admin"
                };
                var res = await userManager.CreateAsync(appUser, "Admin@12");
                await userManager.AddToRoleAsync(appUser, "Admin");
            }

            return Ok("Roles generated");
        }
    }
}
