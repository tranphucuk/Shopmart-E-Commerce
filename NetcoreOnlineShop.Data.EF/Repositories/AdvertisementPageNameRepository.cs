using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class AdvertisementPageNameRepository : EFRepository<AdvertisementPageName, int>, IAdvertisementPageNameRepository
    {
        public AdvertisementPageNameRepository(AppDbContext context) : base(context)
        {
        }
    }
}
