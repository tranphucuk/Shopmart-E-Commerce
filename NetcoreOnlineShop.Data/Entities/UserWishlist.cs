using NetcoreOnlineShop.Data.Interfaces;
using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("UserWishlists")]
    public class UserWishlist : DomainEntity<int>, IDateTracking
    {
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
