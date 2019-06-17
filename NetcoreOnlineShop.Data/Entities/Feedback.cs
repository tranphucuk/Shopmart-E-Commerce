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
    [Table("Feedbacks")]
    public class Feedback : DomainEntity<int>, IDateTracking, ISwitchable
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
