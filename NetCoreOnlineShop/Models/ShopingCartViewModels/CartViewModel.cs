using NetcoreOnlineShop.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ShopingCartViewModels
{
    public class CartViewModel
    {
        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

    }
}
