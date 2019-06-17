using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class BlogImageRepository : EFRepository<BlogImage, int>, IBlogImageRepository
    {
        public BlogImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
