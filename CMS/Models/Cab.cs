using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMS.Models
{
    public enum VehicleType
    {
        AutoRikshaw,
        Car
    }
    public class Cab
    {
        [Key]
        public string VehicleNumber { get; set; }
        public VehicleType VehicleType { get; set; }
        public string VehicleModel { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string DriverId { get; set; }
        public bool Status { get; set; }

    }
}
