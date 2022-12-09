using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Models.ViewModels
{
    [Index(nameof(Email), IsUnique = true)]
    public class RegisterDriverViewModel
    {
        
        [Required]
        [StringLength(15)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(12)]

        [Display(Name = "Licence Number")]
        public string LicenceNumber { get; set; }

        [Required]
        [Display(Name = "Valid Till")]
        public DateTime ValidTo { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        public bool Status { get; set; }
    }
}
