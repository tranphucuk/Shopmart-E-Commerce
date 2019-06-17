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
    [Table("Slides")]
    public class Slide : DomainEntity<int>, IDateTracking, ISwitchable, ISortable
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string Url { get; set; }

        [StringLength(250)]
        [Required]
        public string Image { get; set; }

        [StringLength(250)]
        public string Text { get; set; }

        public int SortOrder { get;set; }
        public Status Status { get;set; }
        public DateTime CreatedDate { get;set; }
        public DateTime ModifiedDate { get;set; }

        [StringLength(25)]
        [Required]
        public string GroupAlias { get; set; }
    }
}
