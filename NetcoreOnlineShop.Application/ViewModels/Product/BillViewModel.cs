using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Product
{
    public class BillViewModel
    {
        public BillViewModel()
        {
            BillDetails = new List<BillDetailViewModel>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { get; set; } //

        public Guid CustomerId { get; set; }

        [StringLength(50)]
        [Required]
        public string CustomerName { get; set; }//

        [StringLength(250)]
        [Required]
        public string CustomerAddress { get; set; }//

        [StringLength(50)]
        [Required]
        public string CustomerPhone { get; set; }//

        [StringLength(50)]
        [Required]
        public string CustomerEmail { get; set; }//

        public BillStatus BillStatus { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public Status Status { get; set; } = Status.Active;

        public ICollection<BillDetailViewModel> BillDetails { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
