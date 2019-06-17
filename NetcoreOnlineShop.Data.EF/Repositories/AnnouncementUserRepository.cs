using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class AnnouncementUserRepository : EFRepository<AnnouncementUser, int>, IAnnouncementUserRepository
    {
        public AnnouncementUserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
