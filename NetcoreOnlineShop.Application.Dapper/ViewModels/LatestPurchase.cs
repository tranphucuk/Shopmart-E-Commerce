using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Dapper.ViewModels
{
    public class LatestPurchase
    {
        public AppUserViewModel User { get; set; }

        public BillViewModel Bill { get; set; }

        public decimal TotalBill { get; set; }

        public decimal Quantity { get; set; }
    }
}
