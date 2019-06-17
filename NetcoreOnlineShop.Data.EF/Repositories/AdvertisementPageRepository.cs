using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class AdvertisementPageRepository : EFRepository<AdvertisementPage, int>, IAdvertisementPageRepository
    {
        public AdvertisementPageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
