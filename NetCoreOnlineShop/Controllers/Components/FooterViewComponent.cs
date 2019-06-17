using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Infrastructure.Enums;
using NetCoreOnlineShop.Models.FooterViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers.Components
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly IFooterService _footerService;
        private readonly IPageService _pageService;
        private IMemoryCache _cache;
        public FooterViewComponent(IFooterService footerService, IPageService pageService, IMemoryCache cache)
        {
            this._footerService = footerService;
            this._pageService = pageService;
            this._cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheEntry = await _cache.GetOrCreateAsync(CacheKeys.FooterViewComponent, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                var listFooters = _footerService.GetAll().OrderBy(x => x.Order);
                var footer = new List<FooterDetails>();
                foreach (var item in listFooters)
                {
                    footer.Add(new FooterDetails()
                    {
                        FooterViewModel = item,
                        PageViewModels = _footerService.GetAllPagesByFooterId(item.Id),
                    });
                }
                return Task.FromResult(footer);
            });
            return View(cacheEntry);
        }
    }
}
