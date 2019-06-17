using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Dapper.ViewModels
{
    public class DataReport
    {
        public IEnumerable< RevenueReportViewModel> Finance { get; set; }

        public IEnumerable<SessionViewModel> Session { get; set; }

        public IEnumerable<TotalRevenueViewModel> TotalRevenue { get; set; }
    }
}
