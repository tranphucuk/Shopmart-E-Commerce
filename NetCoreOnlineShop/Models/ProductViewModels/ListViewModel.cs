using Microsoft.AspNetCore.Mvc.Rendering;
using NetcoreOnlineShop.Application.ViewModels.Ads;
using NetCoreOnlineShop.Models.ShopingCartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ProductViewModels
{
    public class ListViewModel : CatalogViewModel
    {
        public int? PageSize { get; set; }
        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "6", Text="6"},
            new SelectListItem(){Value = "8", Text="8"},
        };

        public List<CartViewModel> CartViewModels { get; set; }

        public List<AdvertisementViewModel> AdvertisementViewModels { get; set; }
    }
}
