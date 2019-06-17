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
    [Table("Blogs")]
    public class Blog : DomainEntity<int>, ISwitchable, IDateTracking, ISortable, IHasSeoMetaData
    {
        public Blog()
        {
            BlogTags = new List<BlogTag>();
            BlogImages = new List<BlogImage>();
        }

        public Blog(string name, string thumbnailImage, string description, string content, Status homeFlag, Status hotFlag,
            string tags, Status status, string seoPageTitle, string seoAlias, string seoMetaKeyword, string seoMetaDescription)
        {
            Name = name;
            Image = thumbnailImage;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            Tags = tags;
            Status = status;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoMetaKeyword;
            SeoDescription = seoMetaDescription;
        }

        public Blog(int id, string name, string thumbnailImage, string description, string content, Status homeFlag, Status hotFlag,
            string tags, Status status, string seoPageTitle, string seoAlias, string seoMetaKeyword, string seoMetaDescription)
        {
            Id = id;
            Name = name;
            Image = thumbnailImage;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            Tags = tags;
            Status = status;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoMetaKeyword;
            SeoDescription = seoMetaDescription;
        }


        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(250)]
        public string Thumbnail { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Status Status { get; set; }

        public Status HomeFlag { get; set; }

        public Status HotFlag { set; get; }

        public int? ViewCount { set; get; }

        public string Tags { get; set; }

        public virtual ICollection<BlogTag> BlogTags { set; get; }

        public virtual ICollection<BlogImage> BlogImages { get; set; }

        [StringLength(250)]
        public string SeoPageTitle { get; set; }

        [StringLength(250)]
        [Column(TypeName = "varchar(250)")]
        public string SeoAlias { get; set; }

        [StringLength(250)]
        public string SeoKeywords { get; set; }

        [StringLength(250)]
        public string SeoDescription { get; set; }
    }
}
