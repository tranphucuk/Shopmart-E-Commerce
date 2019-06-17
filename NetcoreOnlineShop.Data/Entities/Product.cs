using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.Interfaces;
using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("Products")]
    public class Product : DomainEntity<int>, ISwitchable, IDateTracking, IHasSeoMetaData
    {
        public Product()
        {
            ProductTags = new List<ProductTag>();
            ProductImages = new List<ProductImage>();
        }

        public Product(string name, int categoryId, string image, decimal price, decimal? promotionPrice, decimal? originalPrice, string description, string content,
            Status? homeFlag, Status hotFlag, int? viewCount, string tags, int unit, string seoPageTitle, string seoAlias, string seoKeyword, string seoDescription,
            DateTime createdDate, DateTime modifiedDate, Status status)
        {
            Name = name;
            CategoryId = categoryId;
            Image = image;
            Price = price;
            PromotionPrice = promotionPrice;
            OriginalPrice = originalPrice;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            ViewCount = viewCount;
            Tags = tags;
            Unit = unit;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeyword;
            SeoDescription = seoDescription;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            Status = status;
            ProductTags = new List<ProductTag>();
            ProductImages = new List<ProductImage>();
        }

        public Product(int id, string name, int categoryId, string image, decimal price, decimal? promotionPrice, decimal? originalPrice, string description, string content,
            Status? homeFlag, Status hotFlag, int? viewCount, string tags, int unit, string seoPageTitle, string seoAlias, string seoKeyword, string seoDescription,
            DateTime createdDate, DateTime modifiedDate, Status status)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Image = image;
            Price = price;
            PromotionPrice = promotionPrice;
            OriginalPrice = originalPrice;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            ViewCount = viewCount;
            Tags = tags;
            Unit = unit;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeyword;
            SeoDescription = seoDescription;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            Status = status;
            ProductTags = new List<ProductTag>();
            ProductImages = new List<ProductImage>();
        }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string Image { get; set; }

        [DefaultValue(0)]
        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public decimal? OriginalPrice { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        public Status? HomeFlag { get; set; }

        public Status HotFlag { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        public int Unit { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }

        [StringLength(255)]
        public string SeoPageTitle { get; set; }

        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string SeoAlias { get; set; }

        [StringLength(255)]
        public string SeoKeywords { get; set; }

        [StringLength(255)]
        public string SeoDescription { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }

        public virtual ICollection<ProductTag> ProductTags { get; set; }
    }
}
