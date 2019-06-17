using NetcoreOnlineShop.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.OrderHistory
{
    public class OrderDetailViewModel
    {
        public int BillId { get; set; }

        public List<BillDetailViewModel> BillDetails { get; set; }
    }
}
