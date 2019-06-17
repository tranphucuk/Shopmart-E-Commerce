using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        IProductCategoryService _productCategoryService;
        private IMemoryCache _cache;
        public CategoryViewComponent(IProductCategoryService productCategoryService, IMemoryCache cache)
        {
            this._productCategoryService = productCategoryService;
            this._cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheEntry = await _cache.GetOrCreateAsync(CacheKeys.ProductCategoryViewComponent, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                return Task.FromResult(_productCategoryService.GetAllNoMapping());
            });
            return View(cacheEntry);
        }
    }
}
