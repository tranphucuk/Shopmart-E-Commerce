using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.Interfaces;
using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("AdvertisementPages")]
    public class AdvertisementPage : DomainEntity<int>
    {
        public Position Position { get; set; }

        public int AdvertisementId { get; set; }

        public int AdvertisementPageNameId { get; set; }

        [ForeignKey("AdvertisementId")]
        public virtual Advertisement Advertisement { get; set; }

        [ForeignKey("AdvertisementPageNameId")]
        public virtual AdvertisementPageName AdvertisementPageName { get; set; }
    }
}
