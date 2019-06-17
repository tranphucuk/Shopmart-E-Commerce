using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.BlogViewModels
{
    public class BlogDetailViewModel
    {
        public List<BlogViewModel> LatestBlogs { get; set; }

        public List<BlogViewModel> PopularBlogs { get; set; }

        public List<TagViewModel> blogTagViewModels { get; set; }

        public List<TagViewModel> AllTags { get; set; }

        public List<BlogImageViewModel> blogImageViewModels { get; set; }

        public List<BlogViewModel> RelatedBlogs { get; set; }

        public BlogViewModel Blog { get; set; }
    }
}
