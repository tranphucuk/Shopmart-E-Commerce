using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Product
{
    public class BillDetailViewModel
    {
        public int Id { get; set; }

        public int BillId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        public BillViewModel Bill { get; set; }

        public ProductViewModel Product { get; set; }

        public ColorViewModel Color { set; get; }

        public SizeViewModel Size { set; get; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
