using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Ticket
{
    public class SupportTicketViewModel
    {
        public int Id { get; set; }

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
