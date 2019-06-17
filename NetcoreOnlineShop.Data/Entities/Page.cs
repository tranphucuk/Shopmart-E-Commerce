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
    [Table("Pages")]
    public class Page : DomainEntity<int>, IDateTracking, ISwitchable
    {
        [StringLength(256)]
        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName ="varchar(255)")]
        public string Alias { get; set; }

        [Required]
        public string Content { get; set; }

        public Status Status {get;set;}
        public DateTime CreatedDate {get;set;}
        public DateTime ModifiedDate {get;set;}
    }
}
