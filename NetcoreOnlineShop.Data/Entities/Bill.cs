using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.Interfaces;
using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<int>, IDateTracking, ISwitchable
    {
        public Bill()
        {

        }

        public Bill(string customerName, string customerAddress, string customerMobile, string customerMessage,
            BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid customerId)
        {
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerPhone = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
        }

        public Bill(int id, string customerName, string customerAddress, string customerMobile, string customerMessage,
            BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid customerId)
        {
            Id = id;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerPhone = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
        }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { get; set; }

        public Guid CustomerId { get; set; }

        [StringLength(50)]
        [Required]
        public string CustomerName { get; set; }

        [StringLength(250)]
        [Required]
        public string CustomerAddress { get; set; }

        [StringLength(50)]
        [Required]
        public string CustomerPhone { get; set; }

        [StringLength(50)]
        [Required]
        public string CustomerEmail { get; set; }

        public BillStatus BillStatus { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; } = Status.Active;

        public virtual ICollection<BillDetail> BillDetails { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
