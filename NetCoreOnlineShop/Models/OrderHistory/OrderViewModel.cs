using NetcoreOnlineShop.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.OrderHistory
{
    public class OrderViewModel
    {
        public BillViewModel Bill { get; set; }

        public decimal TotalCost { get; set; }
    }
}
