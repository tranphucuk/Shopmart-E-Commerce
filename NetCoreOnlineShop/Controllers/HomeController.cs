using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Slide;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Infrastructure.Enums;
using NetcoreOnlineShop.Utilities.Constants;
using NetCoreOnlineShop.Models;
using NetCoreOnlineShop.Models.ProductViewModels;

namespace NetCoreOnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBlogService _blogService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly ISlideService _slideService;
        private readonly IFeedbackService _feedbackService;
        private readonly IAdvertisementService _advertisementService;
        private readonly ISizeService _sizeService;
        private IMemoryCache _cache;
        public HomeController(IProductService productService, IBlogService blogService,
            IProductCategoryService productCategoryService, ISlideService slideService,
            IFeedbackService feedbackService, ISizeService sizeService, IMemoryCache cache,
            IAdvertisementService advertisementService)
        {
            this._productService = productService;
            this._blogService = blogService;
            this._productCategoryService = productCategoryService;
            this._advertisementService = advertisementService;
            this._slideService = slideService;
            this._feedbackService = feedbackService;
            this._sizeService = sizeService;
            this._cache = cache;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["BodyClass"] = "cms-index-index cms-home-page";

            var cacheEntry = await _cache.GetOrCreateAsync(CacheKeys.HomeController, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                var homeVm = new HomeViewModel()
                {
                    MetaDescription = CommonConstants.HomeMeta.Description,
                    MetaKeyword = CommonConstants.HomeMeta.Keyword,
                    SeoTitle = CommonConstants.HomeMeta.Title,
                    BlogViewModels = _blogService.GetAll().Where(x => x.HomeFlag == Status.Active).ToList(),
                    FeedbackViewModels = _feedbackService.GetAll().Where(x => x.Status == Status.Active).ToList().Take(10).ToList(),
                    ProductViewModels = _productService.GetAll().Where(x => x.Status == Status.Active).ToList(),
                    SlideViewModels = _slideService.GetAll().Where(x => x.Status == Status.Active).OrderBy(x => x.SortOrder).ToList(),
                    ProductCategoryViewModels = _productCategoryService.GetAllNoMapping().Where(x => x.Status == Status.Active).ToList(),
                    AdvertisementViewModels = _advertisementService.GetClientSideAds(CommonConstants.AdsPages.Home)
            };
            return Task.FromResult(homeVm);
        });
            return View(cacheEntry);
    }

    public IActionResult About()
    {
        ViewData["Message"] = "Your application description page.";

        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Message"] = "Your contact page.";

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
}
