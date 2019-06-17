using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Menu
{
    public class MenuViewModel
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

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SortOrder { get; set; }
        public Status Status { get; set; }
    }
}
