using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NetcoreOnlineShop.Application.Implementation;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Infrastructure.Enums;
using NetCoreOnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers.Components
{
    public class HeaderMenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;
        private readonly IBlogService _blogService;
        private IMemoryCache _cache;

        public HeaderMenuViewComponent(IMenuService menuService, IProductCategoryService productCategoryService,
            IProductService productService, IBlogService blogService, IMemoryCache cache)
        {
            this._menuService = menuService;
            this._productCategoryService = productCategoryService;
            this._productService = productService;
            this._blogService = blogService;
            this._cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheEntry = await _cache.GetOrCreateAsync(CacheKeys.HeaderMenuViewComponent, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                HomeViewModel homeViewModel;
                homeViewModel = new HomeViewModel();
                homeViewModel.ProductCategoryViewModels = _productCategoryService.GetAllNoMapping().Where(x => x.Status == Status.Active).ToList();
                homeViewModel.ProductViewModels = _productService.GetAll().Where(x => x.Status == Status.Active).ToList();
                homeViewModel.BlogViewModels = _blogService.GetAll().Where(x => x.Status == Status.Active).ToList();
                homeViewModel.MenuViewModels = _menuService.GetAll().Where(x => x.Status == Status.Active).ToList();

                return Task.FromResult(homeViewModel);
            });
            return View(cacheEntry);
        }
    }
}
