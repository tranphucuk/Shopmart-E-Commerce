using NetcoreOnlineShop.Application.Dapper.ViewModels;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Dapper.Interfaces
{
    public interface IReportService
    {
        Task<DataReport> GetReportsAsync(DateTime fromDate, DateTime toDate);

        Task<PageResult<DataReportPaging>>DataReportPaging(DateTime fromDate, DateTime toDate, int page, int pageSize, int sortOrder);
    }
}
