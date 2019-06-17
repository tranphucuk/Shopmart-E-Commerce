using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.Interfaces;
using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("BillDetails")]
    public class BillDetail : DomainEntity<int>, ISwitchable , IDateTracking
    {
        public BillDetail()
        {

        }

        public BillDetail(int id, int billId, int productId, int quantity, decimal price, int colorId, int sizeId)
        {
            Id = id;
            BillId = billId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            ColorId = colorId;
            SizeId = sizeId;
        }

        public int BillId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        [ForeignKey("BillId")]
        public virtual Bill Bill { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("ColorId")]
        public virtual Color Color { set; get; }

        [ForeignKey("SizeId")]
        public virtual Size Size { set; get; }

        public Status Status { get;set; }
        public DateTime CreatedDate { get;set; }
        public DateTime ModifiedDate { get;set; }
    }
}
