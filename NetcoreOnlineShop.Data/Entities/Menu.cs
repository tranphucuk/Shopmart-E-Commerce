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
    [Table("Menus")]
    public class Menu : DomainEntity<int>, ISwitchable, ISortable, IDateTracking
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string Url { get; set; }

        [StringLength(250)]
        public string Css { get; set; }

        public int ParentId { get; set; }

        public DateTime CreatedDate { get;set;}
        public DateTime ModifiedDate { get;set;}
        public int SortOrder { get;set;}
        public Status Status { get;set;}
    }
}
