using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Announcement
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public virtual ICollection<AnnouncementUserViewModel> AnnouncementUsers { get; set; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
