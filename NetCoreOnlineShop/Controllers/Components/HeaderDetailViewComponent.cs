using Microsoft.AspNetCore.Mvc;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Utilities.Constants;
using NetCoreOnlineShop.Extensions;
using NetCoreOnlineShop.Models.ShopingCartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers.Components
{
    public class HeaderDetailViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public HeaderDetailViewComponent(IProductService productService)
        {
            this._productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            return View(session);
        }
    }
}
