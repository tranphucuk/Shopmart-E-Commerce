using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Infrastructure.Enums;
using NetcoreOnlineShop.Utilities.Constants;
using NetCoreOnlineShop.Models.CommonViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers.Components
{
    public class RightSideBarViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly ITagService _tagService;
        private IMemoryCache _cache;
        public RightSideBarViewComponent(IProductService productService, ITagService tagService, IMemoryCache cache)
        {
            this._productService = productService;
            this._tagService = tagService;
            this._cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheEntry = await _cache.GetOrCreateAsync(CacheKeys.RightSideBarViewComponent, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(3);
                var sidebarVm = new RightSideBarViewModel();
                sidebarVm.Tags = _tagService.GetByType(CommonConstants.productTag).Take(10).ToList();
                return Task.FromResult(sidebarVm);
            });
            return View(cacheEntry);
        }
    }
}
