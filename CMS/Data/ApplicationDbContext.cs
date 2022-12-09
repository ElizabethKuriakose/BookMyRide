using CMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationDriver> ApplicationDrivers { get; set; }
        public DbSet<Cab> Cabs { get; set; }
        public DbSet<RidePath> RidePaths { get; set; }
        public DbSet<Booking> Bookings { get; set; }

    }
}
