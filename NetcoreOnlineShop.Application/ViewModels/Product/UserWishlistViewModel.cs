using NetcoreOnlineShop.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Product
{
    public class UserWishlistViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public AppUserViewModel AppUser { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
