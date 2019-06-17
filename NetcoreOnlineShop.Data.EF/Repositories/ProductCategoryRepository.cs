using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class ProductCategoryRepository : EFRepository<ProductCategory, int>, IProductCategoryRepository
    {
        public ProductCategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
