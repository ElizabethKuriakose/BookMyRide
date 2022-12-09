using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public DateTime RideDate { get; set; }

        public DateTime RideTime { get; set; }

        public RidePath RidePath { get; set; }

        [ForeignKey(nameof(RidePath))]
        public int RouteId { get; set; }

        public string? DriverId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string Uid { get; set; }

        public bool Status { get; set; }
    }
}
