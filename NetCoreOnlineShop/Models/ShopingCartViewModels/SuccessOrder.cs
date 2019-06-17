using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ShopingCartViewModels
{
    public class SuccessOrder
    {
        public BillViewModel Bill { get; set; }
        public List<ProductViewModel> NewArrivals { get; set; }

        public List<TagViewModel> Tags { get; set; }
    }
}
