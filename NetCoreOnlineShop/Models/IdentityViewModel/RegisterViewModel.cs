using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.IdentityViewModel
{
    public class RegisterViewModel : IDateTracking, ISwitchable
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 8, ErrorMessage = "Username must be at least 8 and max 20 characters long.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(maximumLength: 30, ErrorMessage = "Fullname is max 20 characters long.")]
        [Display(Name = "Fullname")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? BirthDay { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 6)]
        public string Address { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
