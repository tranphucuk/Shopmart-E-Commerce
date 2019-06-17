using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Utilities.Dtos;
using NetCoreOnlineShop.Models.Announcements;
using NetCoreOnlineShop.Models.OrderHistory;

namespace NetCoreOnlineShop.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAnnouncementService _announcementService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBillService _billService;
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        public DashboardController(UserManager<AppUser> userManager, IBillService billService,
            IProductService productService, IConfiguration configuration, IAnnouncementService announcementService)
        {
            this._userManager = userManager;
            this._billService = billService;
            this._productService = productService;
            this._configuration = configuration;
            this._announcementService = announcementService;
        }
        public IActionResult Index()
        {
            return Index();
        }

        [HttpGet]
        [Route("/order-history.html")]
        public async Task<IActionResult> OrderHistory(int? pageSize, int page = 1)
        {
            ViewData["BodyClass"] = "dashboard_page";
            if (User.Identity.IsAuthenticated)
            {
                var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                var listBills = _billService.GetBillsByCustomerId(userId);
                var listOrders = new List<OrderViewModel>();
                foreach (var bill in listBills)
                {
                    var order = new OrderViewModel();
                    order.Bill = bill;
                    foreach (var billDetail in bill.BillDetails)
                    {
                        order.TotalCost += billDetail.Price * billDetail.Quantity;
                    }
                    listOrders.Add(order);
                }

                pageSize = _configuration.GetValue<int>("PageSizeOrderHistory");
                var results = listOrders.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
                var pagination = new PageResult<OrderViewModel>()
                {
                    CurentPage = page,
                    PageSize = pageSize.Value,
                    Results = results.ToList(),
                    RowCount = listOrders.Count
                };

                return View(pagination);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/bill-id-{billId}.html")]
        public IActionResult OrderDetail(int billId)
        {
            ViewData["BodyClass"] = "wishlist_page";
            if (User.Identity.IsAuthenticated)
            {
                var billDetails = new OrderDetailViewModel()
                {
                    BillId = billId,
                    BillDetails = _billService.GetBillDetails(billId)
                };
                return View(billDetails);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/wishlist.html")]
        public async Task<IActionResult> Wishlist(int? pageSize, int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                pageSize = _configuration.GetValue<int>("PageSizeWishList");
                var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                var listWishlist = _productService.WishlistProduct(userId, pageSize.Value, page);
                return View(listWishlist);
            }
            var returnUrl = Request.Path;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        [HttpGet]
        [Route("/user-announcement.html")]
        public async Task<IActionResult> Announcement(int? pageSize, int page = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            var announcements = _announcementService.GetUserAnnouncement(userId);
            var listAnnouns = new List<UserAnnouncement>();
            foreach (var announ in announcements)
            {
                var userAnnoun = new UserAnnouncement()
                {
                    Announcement = announ,
                    HasRead = _announcementService.GetUserAnnouncementStatus(announ.Id, userId),
                    DateSent = _announcementService.GetDateSend(announ.Id, userId),
                };
                listAnnouns.Add(userAnnoun);
            }

            var sorted = listAnnouns.OrderByDescending(x => x.DateSent);

            pageSize = _configuration.GetValue<int>("PageSizeAnnouncement");
            var results = sorted.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
            var pagination = new PageResult<UserAnnouncement>()
            {
                CurentPage = page,
                PageSize = pageSize.Value,
                Results = results.ToList(),
                RowCount = sorted.Count()
            };

            return View(pagination);
        }

        [HttpGet]
        [Route("/announcement-{annountId}.html")]
        public async Task<IActionResult> ReadAnnouncement(int annountId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var announcement = _announcementService.GetDetail(annountId);

            var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            _announcementService.UpdateUserAnnouncement(announcement.Id, userId);
            _announcementService.Save();

            var userAnnoun = new UserAnnouncement()
            {
                Announcement = announcement,
                HasRead = true,
                DateSent = _announcementService.GetDateSend(announcement.Id, userId),
            };
            return View(userAnnoun);
        }

        #region Ajax

        [HttpPost]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userName = await _userManager.FindByNameAsync(User.Identity.Name);
            var isRemoved = _productService.RemoveProductInWishlist(productId, userName.Id);
            if (isRemoved)
            {
                _productService.Save();
                return new OkObjectResult(true);
            }
            return new OkObjectResult(false);
        }

        #endregion
    }
}