using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Dapper.ViewModels
{
    public class DataReportPaging
    {
        public DateTime Date { get; set; }

        public int TotalSession { get; set; }

        public decimal Revenue { get; set; }

        public decimal Funds { get; set; }

        public decimal Profit { get; set; }

        public double FlatPercent { get; set; }
    }
}
