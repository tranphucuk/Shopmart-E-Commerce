using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Utilities.Constants;
using NetCoreOnlineShop.Extensions;
using NetCoreOnlineShop.Models.ShopingCartViewModels;
using NetCoreOnlineShop.Services;

namespace NetCoreOnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBillService _billService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly IUserService _userService;
        private readonly ITagService _tagService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEmailSender _emailSender;
        public CartController(IProductService productService, IBillService billService,
            IColorService colorService, ISizeService sizeService, IUserService userService,
            ITagService tagService, IHostingEnvironment hostingEnvironment, IEmailSender emailSender)
        {
            this._productService = productService;
            this._billService = billService;
            this._colorService = colorService;
            this._sizeService = sizeService;
            this._userService = userService;
            this._tagService = tagService;
            this._hostingEnvironment = hostingEnvironment;
            this._emailSender = emailSender;
        }

        [HttpGet]
        [Route("my-cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            ViewData["BodyClass"] = "shopping_cart_page";
            return View();
        }

        [HttpGet]
        [Route("checkout.html", Name = "Checkout")]
        public async Task<IActionResult> Checkout()
        {
            ViewData["BodyClass"] = "checkout_page";
            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (session == null || session.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            var checkoutVm = new CheckoutViewModel();
            if (User.Identity.IsAuthenticated)
            {
                checkoutVm.User = await _userService.FindByNameAsync(User.Identity.Name);
            }
            if (session != null)
            {
                foreach (var item in session)
                {
                    checkoutVm.BasketDetails.Add(new BasketDetails()
                    {
                        Product = item.Product,
                        Color = _colorService.GetColorbyId(item.ColorId),
                        Quantity = item.Quantity,
                        Size = _sizeService.GetSizeById(item.SizeId),
                        Price = item.Price
                    });

                    checkoutVm.TotalBill += item.Price * item.Quantity;
                }
            }
            return View(checkoutVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("checkout.html", Name = "Checkout")]
        public async Task<IActionResult> Checkout(BillViewModel bill)
        {
            ViewData["BodyClass"] = "checkout_page";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Your information is invalid. Please check again.");
                return View();
            }
            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    bill.CustomerId = user.Id;
                }
            }
            else
            {
                bill.CustomerId = Guid.NewGuid();
            }

            foreach (var item in session)
            {
                var billDetail = new BillDetailViewModel();
                billDetail.ProductId = item.Product.Id;
                billDetail.Quantity = item.Quantity;
                billDetail.Price = item.Price;
                billDetail.ColorId = item.ColorId;
                billDetail.SizeId = item.SizeId;
                billDetail.Status = Status.Active;
                bill.BillDetails.Add(billDetail);
            }
            try
            {
                _billService.CreateBill(bill);
                _billService.Save();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Your information is invalid. Please check again.");
            }

            // Send email to customer and admin
            var rootPath = $@"{_hostingEnvironment.WebRootPath}\templates\OrderSuccess\EmailTemplate.html";
            string content = System.IO.File.ReadAllText(rootPath);

            var billid = _billService.GetLatestBillByCustomerId(bill.CustomerId);

            content = content.Replace("{{OrderId}}", billid.Id.ToString());
            content = content.Replace("{{OrderDate}}", billid.CreatedDate.ToString("dd MMM yyyy - hh: mm: ss tt"));
            content = content.Replace("{{PaymentMethod}}", billid.PaymentMethod.ToString());
            content = content.Replace("{{Email}}", billid.CustomerEmail);
            content = content.Replace("{{Fullname}}", billid.CustomerName);
            content = content.Replace("{{Address}}", billid.CustomerAddress);
            content = content.Replace("{{Phone}}", billid.CustomerPhone);
            content = content.Replace("{{Message}}", billid.CustomerMessage);
            decimal totalBill = 0;
            foreach (var item in billid.BillDetails)
            {
                totalBill += item.Quantity * item.Price;
            }
            content = content.Replace("{{TotalCost}}", totalBill.ToString());

            await _emailSender.SendEmailAsync(billid.CustomerEmail, $"ShopMart order confirmation - Order number: #{billid.Id}", content);
            await _emailSender.SendEmailAsync(CommonConstants.mailAdmin, $"ShopMart order confirmation - Order number: #{billid.Id}", content);

            var customId = $"{billid.CustomerId}_{DateTime.Now.Ticks}";
            return RedirectToAction(nameof(CartController.OrderSuccess), new { id = customId });
        }

        [HttpGet]
        [Route("/order-success.html")]
        public IActionResult OrderSuccess(string id)
        {
            ViewData["BodyClass"] = "order_details_page";

            var customerId = id.Split("_")[0];
            var bill = _billService.GetLatestBillByCustomerId(new Guid(customerId));

            var successOrder = new SuccessOrder();
            successOrder.Bill = bill;
            successOrder.NewArrivals = _productService.GetAllNoMapping().OrderByDescending(x => x.CreatedDate).Take(2).ToList();
            successOrder.Tags = _tagService.GetByType(CommonConstants.productTag).Take(10).ToList();

            return View(successOrder);
        }

        #region Ajax
        [HttpGet]
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (session == null)
            {
                session = new List<CartViewModel>();
            }
            return new OkObjectResult(session);
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult(true);
        }

        public IActionResult AddToCart(int productId, int quantity, int colorId, int sizeId)
        {
            var product = _productService.GetById(productId);

            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                if (session.Any(x => x.Product.Id == productId))
                {
                    foreach (var item in session)
                    {
                        if (item.Product.Id == productId)
                        {
                            item.Quantity += quantity;
                            item.Price = product.PromotionPrice ?? product.Price;
                            item.ColorId = colorId;
                            item.SizeId = sizeId;
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    session.Add(new CartViewModel()
                    {
                        Product = product,
                        ColorId = colorId,
                        SizeId = sizeId,
                        Price = product.PromotionPrice ?? product.Price,
                        Quantity = quantity
                    });
                    hasChanged = true;
                }

                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                var cart = new List<CartViewModel>();
                cart.Add(new CartViewModel()
                {
                    Product = product,
                    Quantity = quantity,
                    SizeId = sizeId,
                    ColorId = colorId,
                    Price = product.PromotionPrice ?? product.Price
                });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }
            var basketDetail = new BasketDetails()
            {
                Product = product,
                Color = _colorService.GetColorbyId(colorId),
                Size = _sizeService.GetSizeById(sizeId),
                Quantity = quantity
            };
            return new OkObjectResult(basketDetail);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult UpdateCart(int productId, int quantity, int colorId, int sizeId)
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = _productService.GetById(productId);
                        item.Product = product;
                        item.Quantity = quantity;
                        item.Price = product.PromotionPrice ?? product.Price;
                        item.ColorId = colorId;
                        item.SizeId = sizeId;
                        hasChanged = true;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        public IActionResult LoadColor()
        {
            var colors = _billService.GetAllColor();
            return new OkObjectResult(colors);
        }

        public IActionResult LoadSize()
        {
            var sizes = _billService.GetAllSize();
            return new OkObjectResult(sizes);
        }

        [HttpGet]
        public IActionResult CheckoutTooltip()
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>(CommonConstants.CartSession);
            if (session == null || session.Count == 0)
            {
                return new OkObjectResult(false);
            }
            return new OkObjectResult(true);
        }
        #endregion
    }
}