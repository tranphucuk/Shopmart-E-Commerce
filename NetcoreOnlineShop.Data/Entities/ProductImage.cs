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
    [Table("ProductImages")]
    public class ProductImage : DomainEntity<int>, ISwitchable
    {
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [StringLength(250)]
        public string Path { get; set; }

        public double? Size { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }
        public Status Status { get; set; }
    }
}
