using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.System
{
    public class AppUserViewModel
    {
        public AppUserViewModel()
        {
            Roles = new List<string>();
        }

        public Guid Id { get; set; }

        [StringLength(20)]
        public string UserName { get; set; }

        public string Password { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        [StringLength(50)]
        [Required]
        public string FullName { get; set; }

        public decimal Balance { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(256)]
        public string Avatar { get; set; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public List<string> Roles { get; set; }
    }
}
