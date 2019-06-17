using System.ComponentModel.DataAnnotations;

namespace NetCoreOnlineShop.Models.IdentityViewModel
{
    public class EmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}