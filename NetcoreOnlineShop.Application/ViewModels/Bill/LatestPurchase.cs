using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Bill
{
    public class LatestPurchase
    {
        public BillViewModel Bill { get; set; }

        public decimal TotalCost { get; set; }

        public decimal Quantity { get; set; }
    }
}
