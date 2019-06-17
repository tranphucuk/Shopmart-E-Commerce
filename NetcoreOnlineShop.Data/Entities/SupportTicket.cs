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
    [Table("SupportTickets")]
    public class SupportTicket : DomainEntity<int>, IDateTracking, ISwitchable
    {
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(30)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int BillId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
