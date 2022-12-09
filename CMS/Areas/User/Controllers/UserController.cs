using CMS.Data;
using CMS.Models;
using CMS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Booking()
        {
            var model = new BookingViewModel()
            {
                RidePaths = await db.RidePaths.ToListAsync()
            };
            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> Booking(BookingViewModel model)
        {
            model.RidePaths = await db.RidePaths.ToListAsync();
            //return Json(model);
            var user = await userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
                return View(model);

            var routeId = int.Parse(model.Source);

            db.Bookings.Add(new Booking()
            {
                RideDate = model.RideDate,
                RideTime = model.RideTime,
                RouteId = routeId,
                Uid = user.Id,
               
            });
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ConfirmRide), "User", new { Area = "User", id = routeId });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmRide(int id)
        {
            var Routes = db.RidePaths.Where(i => i.Id == id).First();
        
            return View(new RidePathViewModel
            {
                Source = Routes.Source,
                Destination = Routes.Destination,
                Cost = Routes.Cost

            });
        }
        public async Task<IActionResult> ViewHistory()
        {
            //var user = await userManager.GetUserAsync(User);
            //return View(db.Bookings.Where(i => i.Uid == user.Id).ToList());



            var x = db.Bookings.Include(i => i.RidePath).Where(i => i.Uid == userManager.GetUserAsync(User).Result.Id);
            return View(x);
        }
        public async Task<IActionResult> Payment()
        {
            var model = new BookingViewModel()
            {
                RidePaths = await db.RidePaths.ToListAsync()
            };
            return View(model);
        }

    }
}
