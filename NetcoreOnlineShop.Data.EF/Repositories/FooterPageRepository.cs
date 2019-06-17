using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class FooterPageRepository : EFRepository<FooterPage, int>, IFooterPageRepository
    {
        public FooterPageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
