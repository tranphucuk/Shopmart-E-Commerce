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
    [Table("ProductCategories")]
    public class ProductCategory : DomainEntity<int>, IHasSeoMetaData, ISwitchable, ISortable, IDateTracking
    {
        public ProductCategory()
        {
            Products = new List<Product>();
        }

        public ProductCategory(int id,string name, string description, int? parentId, int? homeOrder, string image, DateTime createdDate, DateTime modifiedDate, Status? homeFlag, int sortOrder,
            string seoPageTitle, string seoAlias, string seoKeywords, string seoDescription, Status status)
        {
            Id = id;
            Name = name;
            Description = description;
            ParentId = parentId;
            HomeOrder = homeOrder;
            Image = image;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            HomeFlag = homeFlag;
            SortOrder = sortOrder;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeywords;
            SeoDescription = seoDescription;
            Status = status;
            Products = new List<Product>();
        }

        [StringLength(255)]
        [Required]
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

        [Column(TypeName = "varchar(255)")]
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

        public virtual ICollection<Product> Products { get; set; }
    }
}
