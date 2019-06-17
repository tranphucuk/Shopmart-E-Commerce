using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Ads;
using NetcoreOnlineShop.Application.ViewModels.Menu;
using NetcoreOnlineShop.Application.ViewModels.Slide;
using NetCoreOnlineShop.Models.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models
{
    public class HomeViewModel
    {
        public List<MenuViewModel> MenuViewModels { get; set; }

        public List<SlideViewModel> SlideViewModels { get; set; }

        public List<ProductCategoryViewModel> ProductCategoryViewModels { get; set; }

        public List<ProductViewModel> ProductViewModels { get; set; }

        public List<FeedbackViewModel> FeedbackViewModels { get; set; }

        public List<BlogViewModel> BlogViewModels { get; set; }

        public List<AdvertisementViewModel> AdvertisementViewModels { get; set; }

        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string SeoTitle { get; set; }
    }
}
