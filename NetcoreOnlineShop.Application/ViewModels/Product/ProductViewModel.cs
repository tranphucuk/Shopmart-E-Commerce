using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }//

        [Required]
        public int CategoryId { get; set; }//

        [Required]
        [StringLength(255)]
        public string Image { get; set; }//

        [DefaultValue(0)]
        public decimal Price { get; set; }//

        public decimal? PromotionPrice { get; set; } //

        public decimal? OriginalPrice { get; set; }//

        [Required]
        [StringLength(500)]
        public string Description { get; set; }//

        [Required]
        public string Content { get; set; }//

        public Status? HomeFlag { get; set; } //

        public Status HotFlag { get; set; }//

        public int? ViewCount { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }//

        public int Unit { get; set; } //

        public ProductCategoryViewModel ProductCategory { get; set; }

        [StringLength(255)]
        public string SeoPageTitle { get; set; }//

        [StringLength(255)]
        public string SeoAlias { get; set; } //

        [StringLength(255)]
        public string SeoKeywords { get; set; }//

        [StringLength(255)]
        public string SeoDescription { get; set; }//

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Status Status { get; set; }//

        public ICollection<ProductImageViewModel> ProductImages { get; set; }

        public ICollection<ProductTagViewModel> ProductTags { get; set; }
    }
}
