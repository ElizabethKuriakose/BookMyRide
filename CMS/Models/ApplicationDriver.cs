using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    public class ApplicationDriver
    {
        [Key]
        public string LicenceNumber { get; set; }

        public DateTime ValidTo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        public bool Status { get; set; }

    }
}
