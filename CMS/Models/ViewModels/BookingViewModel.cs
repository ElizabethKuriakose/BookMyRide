using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models.ViewModels
{
    public class BookingViewModel
    {
        public DateTime RideDate { get; set; }

        public DateTime RideTime { get; set; }

        public string Source { get; set; }
        public string Destination { get; set; }

        public double RideAmount { get; set; } = 500;

        public int RoutId { get; set; }
        public IEnumerable<RidePath>? RidePaths { get; set; }

        public IEnumerable<ApplicationUser>? ApplicationUsers { get; set; }

    }
}
