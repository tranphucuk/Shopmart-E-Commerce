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
    [Table("Advertisements")]
    public class Advertisement : DomainEntity<int>, IDateTracking, ISwitchable, ISortable
    {
        public Advertisement()
        {
            AdvertisementPages = new List<AdvertisementPage>();
        }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string Description { get; set; }

        [StringLength(250)]
        [Required]
        public string Image { get; set; }

        [StringLength(250)]
        [Required]
        public string Url { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SortOrder { get; set; }
        public virtual ICollection<AdvertisementPage> AdvertisementPages { get; set; }
    }
}
