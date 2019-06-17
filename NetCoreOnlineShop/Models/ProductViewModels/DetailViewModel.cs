using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductViewModel ProductViewModel { get; set; }
        public List<ProductImageViewModel> productImageViewModels { get; set; }

        public ProductCategoryViewModel ProductCategoryViewModel { get; set; }

        public List<ColorViewModel> colorViewModels { get; set; }

        public List<SizeViewModel> sizeViewModels { get; set; }

        public List<TagViewModel> TagViewModels { get; set; }

        public List<ProductViewModel> RelatedProducts { get; set; }

        public List<ProductViewModel> SaleProducts { get; set; }

        public bool IsInWishList { get; set; }

    }
}
