using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class AdvertisementRepository : EFRepository<Advertisement, int>, IAdvertisementRepository
    {
        public AdvertisementRepository(AppDbContext context) : base(context)
        {
        }
    }
}
