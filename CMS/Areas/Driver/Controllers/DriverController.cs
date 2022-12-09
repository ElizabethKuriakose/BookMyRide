using CMS.Data;
using CMS.Models;
using CMS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace CMS.Areas.Driver.Controllers
{
    [Area("Driver")]
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DriverController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            //var status = db.ApplicationDrivers.Where(i => i.St)
            var dri = await db.ApplicationDrivers.FirstOrDefaultAsync(i => i.Status == true && i.UserId == user.Id);

            if(dri == null)
            {
                return Ok("You Are Not Varified");
            }
            else {
                var model = new BookingViewModel()
                {
                    RidePaths = await db.RidePaths.ToListAsync(),
                    ApplicationUsers = await db.ApplicationUsers.ToListAsync(),

                };
                return View(db.Bookings.Where(i => i.Status == false).ToList());
            }
           
        }

        [HttpGet]
        public IActionResult CabRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CabRegistration(CabRegistrationViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            db.Cabs.Add(new Cab()
            {
                VehicleNumber = model.VehicleNumber,
                VehicleType = model.VehicleType,
                VehicleModel = model.VehicleModel,
                DriverId = user.Id,
            });

            await db.SaveChangesAsync();

            //ModelState.AddModelError("", "An Error Occured!!");
            return View(model);
        }

        public async Task<IActionResult> CommitRide(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var bk = await db.Bookings.FindAsync(id);
            if (bk == null)
            {
                return NotFound();
            }
            bk.DriverId = user.FirstName;
            bk.Status = true;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
