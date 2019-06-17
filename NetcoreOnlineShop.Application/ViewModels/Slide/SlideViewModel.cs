using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Slide
{
    public class SlideViewModel
    {
        public int Id { get; set; }

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

        public int SortOrder { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [StringLength(25)]
        [Required]
        public string GroupAlias { get; set; }
    }
}
