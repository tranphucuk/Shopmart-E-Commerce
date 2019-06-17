using NetcoreOnlineShop.Application.ViewModels.Blog;
using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }

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

        public ICollection<BlogTagViewModel> BlogTags { set; get; }

        public ICollection<BlogImageViewModel> BlogImages { get; set; }

        [StringLength(250)]
        public string SeoPageTitle { get; set; }

        [StringLength(250)]
        public string SeoAlias { get; set; }

        [StringLength(250)]
        public string SeoKeywords { get; set; }

        [StringLength(250)]
        public string SeoDescription { get; set; }
    }
}
