using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Dapper.ViewModels
{
    public class DetailThisMonth
    {
        public int NewAccount { get; set; }
        public int NewProduct { get; set; }
        public int Sold { get; set; }
        public int CompletedBill { get; set; }
    }
}
