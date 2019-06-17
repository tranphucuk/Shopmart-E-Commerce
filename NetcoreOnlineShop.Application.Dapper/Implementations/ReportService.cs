using Dapper;
using Microsoft.Extensions.Configuration;
using NetcoreOnlineShop.Application.Dapper.Interfaces;
using NetcoreOnlineShop.Application.Dapper.ViewModels;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Dapper.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IConfiguration _config;
        public ReportService(IConfiguration config)
        {
            this._config = config;
        }

        public async Task<PageResult<DataReportPaging>> DataReportPaging(DateTime fromDate, DateTime toDate, int page, int pageSize, int sortOrder)
        {
            using (var sql = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await sql.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                var now = DateTime.Now;

                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                dynamicParameters.Add("@fromDate", fromDate == default(DateTime) ? firstDayOfMonth.ToString("MM/dd/yyyy") : fromDate.ToString("MM/dd/yyyy"));
                dynamicParameters.Add("@toDate", fromDate == default(DateTime) ? lastDayOfMonth.ToString("MM/dd/yyyy") : toDate.ToString("MM/dd/yyyy"));

                try
                {
                    var query = await sql.QueryAsync<DataReportPaging>("DataReport", dynamicParameters, commandType: System.Data.CommandType.StoredProcedure);

                    switch (sortOrder)
                    {
                        case 0:
                            query = query.OrderByDescending(x => x.Date);
                            break;
                        case 1:
                            query = query.OrderByDescending(x => x.TotalSession);
                            break;
                        case 2:
                            query = query.OrderByDescending(x => x.Revenue);
                            break;
                        case 3:
                            query = query.OrderByDescending(x => x.Funds);
                            break;
                        case 4:
                            query = query.OrderByDescending(x => x.Profit);
                            break;
                        default:
                            break;
                    }

                    var total = query.Count();
                    var data = query.Skip((page - 1) * pageSize).Take(pageSize);

                    var pageResult = new PageResult<DataReportPaging>()
                    {
                        CurentPage = page,
                        PageSize = pageSize,
                        Results = data.ToList(),
                        RowCount = total
                    };

                    return pageResult;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<DataReport> GetReportsAsync(DateTime fromDate, DateTime toDate)
        {
            using (var sql = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await sql.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                var now = DateTime.Now;

                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                dynamicParameters.Add("@fromDate", fromDate == default(DateTime) ? firstDayOfMonth.ToString("MM/dd/yyyy") : fromDate.ToString("MM/dd/yyyy"));
                dynamicParameters.Add("@toDate", fromDate == default(DateTime) ? lastDayOfMonth.ToString("MM/dd/yyyy") : toDate.ToString("MM/dd/yyyy"));

                try
                {
                    var data = new DataReport();
                    data.Finance = await sql.QueryAsync<RevenueReportViewModel>
                        ("GetRevenueDaily", dynamicParameters, commandType: System.Data.CommandType.StoredProcedure);
                    data.Session = await sql.QueryAsync<SessionViewModel>("SumSessionPerDay", dynamicParameters,
                        commandType: System.Data.CommandType.StoredProcedure);

                    return data;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
