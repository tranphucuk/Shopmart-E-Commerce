using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("AppUserActivities")]
    public class AppUserActivity : DomainEntity<int>
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public DateTime LastSession { get; set; }

        [MaxLength(50)]
        public string Device { get; set; }

        [MaxLength(20)]
        public string IPAddress { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
    }
}
