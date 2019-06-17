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
    [Table("BlogImages")] 
    public class BlogImage : DomainEntity<int>, ISwitchable
    {
        public int BlogId { get; set; }

        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; } 

        [StringLength(250)]
        public string Path { get; set; }

        public decimal? Size { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }
        public Status Status { get; set; }
    }
}
