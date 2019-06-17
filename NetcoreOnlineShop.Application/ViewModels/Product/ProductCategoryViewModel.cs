using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [StringLength(255)]
        [Required]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public int? HomeOrder { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [StringLength(255)]
        [Required]
        public string SeoPageTitle { get; set; }

        [StringLength(255)]
        public string SeoAlias { get; set; }

        [StringLength(255)]
        [Required]
        public string SeoKeywords { get; set; }

        [StringLength(255)]
        [Required]
        public string SeoDescription { get; set; }

        public Status? HomeFlag { get; set; }

        public Status Status { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }
    }
}
