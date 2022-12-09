using CMS.Data;
using CMS.Models;
using CMS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

     
        public async Task<IActionResult> VerifyCab()
        {
           
            var model = new CabRegistrationViewModel()
            {
                ApplicationUsers = await db.ApplicationUsers.ToListAsync()
            };
            return View(db.Cabs.Where(i => i.Status == false).ToList());
        }

 
        public async Task<IActionResult> VerifyCab1(string id)
        {
            var cab = await db.Cabs.FindAsync(id);
            if (cab == null)
            {
                return NotFound();
            }

            cab.Status = true;
            await db.SaveChangesAsync();
            return Ok("Verified");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyDriver()
        {
            var model = new CabRegistrationViewModel()
            {
                ApplicationUsers = await db.ApplicationUsers.ToListAsync()
            };
            return View(db.ApplicationDrivers.Where(i => i.Status == true).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> VerifyDriver1(string id)
        {
            var driver = await db.ApplicationDrivers.FindAsync(id);
            //Console.WriteLine(driver.UserId);
            if (driver == null)
            {
                return NotFound();
            }

            driver.Status = true;
            await db.SaveChangesAsync();
            return Ok("Verified");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var cab = await db.Cabs.FindAsync(id);
            if (cab == null)
            {
                return NotFound();
            }
            db.Cabs.Remove(cab);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddLocation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation(RidePathViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            db.RidePaths.Add(new RidePath()
            {
                Source = model.Source,
                Destination = model.Destination,
                Cost = model.Cost
            });
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit()
        {
            return View(db.RidePaths.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> EditLocation(int id)
        {
            var loc = await db.RidePaths.FindAsync(id);
            if (loc == null)
            {
                return NotFound();
            }
            return View(new RidePathViewModel()
            {
                Source = loc.Source,
                Destination = loc.Destination,
                Cost = loc.Cost
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditLocation(int id, RidePathViewModel model)
        {
            var loc = await db.RidePaths.FindAsync(id);
            if (loc == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            loc.Source = model.Source;
            loc.Destination = model.Destination;
            loc.Cost = model.Cost;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Edit));
        }

      
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var loc = await db.RidePaths.FindAsync(id);
            if (loc == null)
            {
                return NotFound();
            }
            db.RidePaths.Remove(loc);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Edit));
        }
    }
}
