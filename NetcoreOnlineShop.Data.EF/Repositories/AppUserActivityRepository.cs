using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class AppUserActivityRepository : EFRepository<AppUserActivity, int>, IAppUserActivityRepository
    {
        public AppUserActivityRepository(AppDbContext context) : base(context)
        {
        }
    }
}
