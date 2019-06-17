using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class FooterRepository : EFRepository<Footer, int>, IFooterRepository
    {
        public FooterRepository(AppDbContext context) : base(context)
        {
        }
    }
}
