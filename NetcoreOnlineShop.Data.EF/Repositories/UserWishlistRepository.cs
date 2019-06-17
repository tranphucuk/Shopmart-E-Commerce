using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class UserWishlistRepository : EFRepository<UserWishlist, int>, IUserWishlistRepository
    {
        public UserWishlistRepository(AppDbContext context) : base(context)
        {
        }
    }
}
