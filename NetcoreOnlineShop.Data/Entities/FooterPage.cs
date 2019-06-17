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
    [Table("FooterPages")]
    public class FooterPage : DomainEntity<int>, ISwitchable
    {
        public int PageId { get; set; }
        public int FooterId { get; set; }

        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }

        [ForeignKey("FooterId")]
        public virtual Footer Footer { get; set; }
        public Status Status { get; set; }
    }
}
