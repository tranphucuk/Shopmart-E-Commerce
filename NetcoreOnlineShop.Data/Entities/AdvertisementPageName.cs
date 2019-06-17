using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("AdvertisementPageNames")]
    public class AdvertisementPageName : DomainEntity<int>
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}