using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.System
{
    public class FunctionViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(128)]
        public string ParentId { get; set; }

        public string IconCss { get; set; }

        public int SortOrder { get; set; }
        public Status Status { get; set; }
    }
}
