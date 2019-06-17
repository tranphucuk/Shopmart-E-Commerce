using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.IdentityViewModel
{
    public class ExternalLoginViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime? BirthDay { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }
    }
}
