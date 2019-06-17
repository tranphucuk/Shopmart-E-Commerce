using Microsoft.AspNetCore.Mvc.Rendering;
using NetcoreOnlineShop.Application.ViewModels.Ads;
using NetCoreOnlineShop.Models.ShopingCartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ProductViewModels
{
    public class GridViewModel : CatalogViewModel
    {
        public int? PageSize { get; set; }
        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "12", Text="12"},
            new SelectListItem(){Value = "15", Text="15"},
        };

        public List<CartViewModel> CartViewModels { get; set; }

        public List<AdvertisementViewModel> AdvertisementViewModels { get; set; }
    }
}
