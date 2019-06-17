using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.Interfaces;
using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("Announcements")]
    public class Announcement : DomainEntity<int>, IDateTracking, ISwitchable
    {
        public Announcement()
        {
            AnnouncementUsers = new List<AnnouncementUser>();
        }

        [StringLength(250)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public virtual ICollection<AnnouncementUser> AnnouncementUsers { get; set; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
