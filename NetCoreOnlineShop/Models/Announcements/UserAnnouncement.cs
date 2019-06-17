using NetcoreOnlineShop.Application.ViewModels.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.Announcements
{
    public class UserAnnouncement
    {
        public AnnouncementViewModel Announcement { get; set; }

        public bool HasRead { get; set; }

        public DateTime DateSent { get; set; }
    }
}
