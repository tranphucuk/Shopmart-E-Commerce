﻿using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ShopingCartViewModels
{
    public class BasketDetails
    {
        public ProductViewModel Product { get; set; }

        public ColorViewModel Color { get; set; }

        public SizeViewModel Size { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        //public BillViewModel Bill { get; set; }
    }
}
