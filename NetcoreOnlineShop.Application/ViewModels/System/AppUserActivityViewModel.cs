using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.System
{
    public class AppUserActivityViewModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public DateTime LastSession { get; set; }

        [MaxLength(50)]
        public string Device { get; set; }

        [MaxLength(20)]
        public string IPAddress { get; set; }

        public AppUserViewModel AppUser { get; set; }
    }
}
