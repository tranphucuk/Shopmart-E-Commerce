using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Utilities.Constants;
using NetcoreOnlineShop.Utilities.Dtos;
using NetCoreOnlineShop.Extensions;
using NetCoreOnlineShop.Models.ProductViewModels;
using NetCoreOnlineShop.Models.ShopingCartViewModels;
using static NetcoreOnlineShop.Utilities.Constants.CommonConstants;

namespace NetCoreOnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IAdvertisementService _advertisementService;
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly ITagService _tagService;
        private readonly IBlogService _blogService;
        private readonly UserManager<AppUser> _userManager;
        public ProductController(IProductService productService, IProductCategoryService productCategoryService,
            IConfiguration configuration, ISizeService sizeService, IColorService colorService,
            ITagService tagService, IBlogService blogService, IAdvertisementService advertisementService, UserManager<AppUser> userManager)
        {
            this._productCategoryService = productCategoryService;
            this._advertisementService = advertisementService;
            this._productService = productService;
            this._configuration = configuration;
            this._colorService = colorService;
            this._sizeService = sizeService;
            this._tagService = tagService;
            this._blogService = blogService;
            this._userManager = userManager;
        }

        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("category-{alias}-{id}.html")] // View products by grid view
        public IActionResult Catalog_Grid(int id, int? pageSize, string sortBy, string priceRange, int page = 1)
        {
            ViewData["BodyClass"] = "shop_grid_page";
            var catalog = new GridViewModel();
            if (pageSize == null)
            {
                pageSize = _configuration.GetValue<int>("PageSize_Grid");
            }

            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            var results = new PageResult<ProductViewModel>();
            if (priceRange != null)
            {
                catalog.IsSorted = true;
                catalog.PriceSort = priceRange;
                TempData["PriceSort"] = priceRange;
                results = _productService.SortProductByConditions(id, priceRange, catalog.SortType, page, pageSize.Value);
            }
            else
            {
                results = _productService.SortProductByConditions(id, string.Empty, catalog.SortType, page, pageSize.Value);
            }

            catalog.Data = results;
            catalog.ProductCategory = _productCategoryService.GetById(id);
            catalog.AllCategories = _productCategoryService.GetAll();
            catalog.Blogs = _blogService.GetAll().Take(2).ToList();
            catalog.ProductTags = _productService.GetAllTags().Take(10).ToList();

            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                catalog.CartViewModels = session;
            }

            //catalog.AdvertisementViewModels = _advertisementService.GetClientSideAds(AdsPages.Category);

            return View(catalog);
        }

        [Route("category-{alias}-{id}/shop_list.html")] //View product by list view
        public IActionResult Shop_List(int id, int? pageSize, string sortBy, string priceRange, int page = 1)
        {
            ViewData["BodyClass"] = "shop_list_page";
            var catalog = new ListViewModel();
            if (pageSize == null)
            {
                pageSize = _configuration.GetValue<int>("PageSize_List");
            }

            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            var results = new PageResult<ProductViewModel>();
            if (priceRange != null)
            {
                catalog.IsSorted = true;
                catalog.PriceSort = priceRange;
                TempData["PriceSort"] = priceRange;
                results = _productService.SortProductByConditions(id, priceRange, catalog.SortType, page, pageSize.Value);
            }
            else
            {
                results = _productService.SortProductByConditions(id, string.Empty, catalog.SortType, page, pageSize.Value);
            }

            catalog.Data = results;
            catalog.ProductCategory = _productCategoryService.GetById(id);
            catalog.AllCategories = _productCategoryService.GetAll();
            catalog.Blogs = _blogService.GetAll().Take(4).ToList();
            catalog.ProductTags = _productService.GetAllTags().Take(10).ToList();

            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                catalog.CartViewModels = session;
            }

            //catalog.AdvertisementViewModels = _advertisementService.GetClientSideAds(AdsPages.Category);

            return View(catalog);
        }

        [Route("product-{alias}-{id}.html")]
        public async Task<IActionResult> Details(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var product = _productService.GetById(id);
            var productbyCategories = _productService.GetProductsByCategoryId(product.CategoryId).Where(x => x.Status == Status.Active);

            var details = new DetailViewModel();
            details.ProductViewModel = product;
            details.ProductCategoryViewModel = _productCategoryService.GetById(product.CategoryId);
            details.colorViewModels = _colorService.GetAll();
            details.sizeViewModels = _sizeService.GetAll();
            details.productImageViewModels = _productService.GetProductImages(product.Id);
            details.RelatedProducts = productbyCategories.Take(7).ToList();
            details.SaleProducts = productbyCategories.Where(x => x.PromotionPrice != null).Take(7).ToList();
            details.TagViewModels = _productService.GetTagsByProduct(product.Id);
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    details.IsInWishList = _productService.IsExistedInWishlist(id, user.Id);
                }
            }
            return View(details);
        } // product detail

        [Route("search.html")]
        public IActionResult Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            var search = new SearchViewModel();
            if (pageSize == null)
            {
                pageSize = _configuration.GetValue<int>("PageSize");
            }

            search.PageSize = pageSize;
            search.SortType = sortBy;
            search.Data = _productService.GetProductbyKeyword(keyword, search.SortType, pageSize.Value, page);
            search.Keyword = keyword;
            return View(search);
        } // search product

        [Route("products-by-tag-{tagId}.html")]
        public IActionResult ProductsByTag(string tagId, int? pageSize, string sortBy, int page = 1)
        {
            if (pageSize == null)
            {
                pageSize = int.Parse(_configuration["PageSize"]);
            }
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            ViewData["TagId"] = tagId;
            ViewData["TagName"] = _tagService.FindById(tagId).Name;

            var productsByTag = new SearchViewModel();
            productsByTag.SortType = sortBy;
            var products = _productService.GetProductsByTagId(tagId, productsByTag.SortType, pageSize.Value, page);

            productsByTag.Data = products;
            return View(productsByTag);
        }

        // New Arrival
        [Route("new-arrival-products.html")]
        public IActionResult NewArrival(int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
            {
                pageSize = int.Parse(_configuration["PageSize"]);
            }
            var arrivalProducts = new SearchViewModel();
            arrivalProducts.SortType = sortBy;
            arrivalProducts.Data = _productService.GetProductsType(pageSize.Value, arrivalProducts.SortType, page, ProductType.NewArrival);
            return View(arrivalProducts);
        }

        // Sepcial Offer
        [Route("special-offer-products.html")]
        public IActionResult SpeicalOffer(int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
            {
                pageSize = int.Parse(_configuration["PageSize"]);
            }
            var SpeicalOffer = new SearchViewModel();
            SpeicalOffer.SortType = sortBy;
            SpeicalOffer.Data = _productService.GetProductsType(pageSize.Value, SpeicalOffer.SortType, page, ProductType.SpeicalOffer);
            return View(SpeicalOffer);
        }

        // best Seller
        [Route("best-seller-products.html")]
        public IActionResult BestSeller(int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
            {
                pageSize = int.Parse(_configuration["PageSize"]);
            }
            var BestSeller = new SearchViewModel();
            BestSeller.SortType = sortBy;
            BestSeller.Data = _productService.GetProductsType(pageSize.Value, BestSeller.SortType, page, ProductType.BestSeller);
            return View(BestSeller);
        }

        #region Ajax
        //quick view product
        [HttpGet]
        public IActionResult QuickView(int productId)
        {
            var product = _productService.GetById(productId);
            var relatedProducts = _productService.GetProductsByCategoryIdNoMapping(product.CategoryId).Where(x => x.Id != productId).Take(5).ToList();
            relatedProducts.Insert(0, product);

            var quickViewModel = new QuickViewProduct()
            {
                RelatedProducts = relatedProducts,
                ProductViewModel = product
            };
            return new OkObjectResult(quickViewModel);
        }

        [HttpGet]
        public IActionResult ChangeProduct(int productId)
        {
            var product = _productService.GetById(productId);
            return new OkObjectResult(product);
        }

        //Add to wishlist 
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                try
                {
                    _productService.AddToWishlist(productId, user.Id);
                    _productService.Save();
                    return new OkObjectResult(true);
                }
                catch (Exception ex)
                {
                    return new OkObjectResult(false);
                }
            }
            return new OkObjectResult(false);
        }
        #endregion
    }
}