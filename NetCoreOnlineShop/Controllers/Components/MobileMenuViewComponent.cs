using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers.Components
{
    public class MobileMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryService _productCategoryService;
        private IMemoryCache _cache;

        public MobileMenuViewComponent(IProductCategoryService productCategoryService, IMemoryCache cache)
        {
            this._productCategoryService = productCategoryService;
            this._cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheEntry = await _cache.GetOrCreateAsync(CacheKeys.MobileMenuViewComponent, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                return Task.FromResult(_productCategoryService.GetAllNoMapping());
            });

            return View(cacheEntry);
        }
    }
}
