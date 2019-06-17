using AutoMapper;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Ads;
using NetcoreOnlineShop.Application.ViewModels.Announcement;
using NetcoreOnlineShop.Application.ViewModels.Blog;
using NetcoreOnlineShop.Application.ViewModels.Footer;
using NetcoreOnlineShop.Application.ViewModels.NewsLetter;
using NetcoreOnlineShop.Application.ViewModels.Page;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Application.ViewModels.Slide;
using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Application.ViewModels.Ticket;
using NetcoreOnlineShop.Data.Entities;

namespace NetcoreOnlineShop.Application.AutoMapper
{
    public class ViewModelToDomainMapping : Profile
    {
        public ViewModelToDomainMapping()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>().ConstructUsing(c => new ProductCategory(c.Id, c.Name, c.Description, c.ParentId,
                c.HomeOrder, c.Image, c.CreatedDate, c.ModifiedDate, c.HomeFlag, c.SortOrder, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription, c.Status));

            CreateMap<ProductViewModel, Product>().ConstructUsing(p => new Product(p.Name, p.CategoryId, p.Image, p.Price, p.PromotionPrice, p.OriginalPrice,
                p.Description, p.Content, p.HomeFlag, p.HotFlag, p.ViewCount, p.Tags, p.Unit, p.SeoPageTitle, p.SeoAlias, p.SeoKeywords, p.SeoDescription, p.CreatedDate, p.ModifiedDate,
                p.Status));

            CreateMap<PermissionViewModel, Permission>().ConstructUsing(p => new Permission(p.RoleId, p.FunctionId,
                p.CanRead, p.CanCreate, p.CanUpdate, p.CanDelete));

            CreateMap<BillViewModel, Bill>().ConstructUsing(b => new Bill(b.Id, b.CustomerName, b.CustomerAddress, b.CustomerPhone,
                b.CustomerMessage, b.BillStatus, b.PaymentMethod, b.Status, b.CustomerId));

            CreateMap<BillDetailViewModel, BillDetail>().ConstructUsing(b => new BillDetail(b.Id, b.BillId, b.ProductId, b.Quantity, b.Price,
                b.ColorId, b.SizeId));

            CreateMap<WholePriceViewModel, WholePrice>();
            CreateMap<FeedbackViewModel, Feedback>();
            CreateMap<SlideViewModel, SlideViewModel>();
            CreateMap<BlogViewModel, Blog>();
            CreateMap<BlogImageViewModel, BlogImage>();
            CreateMap<TagViewModel, Tag>();
            CreateMap<BlogTagViewModel, BlogTag>();
            CreateMap<ContactViewModel, Contact>();
            CreateMap<PageViewModel, Page>();
            CreateMap<FooterViewModel, Footer>();
            CreateMap<FooterPageViewModel, FooterPage>();
            CreateMap<AppUserActivityViewModel, AppUserActivity>();
            CreateMap<AdvertisementViewModel, Advertisement>();
            CreateMap<AdvertisementPageNameViewModel, AdvertisementPageName>();
            CreateMap<AdvertisementPageViewModel, AdvertisementPage>();
            CreateMap<AnnouncementViewModel, Announcement>();
            CreateMap<SupportTicketViewModel, SupportTicket>();
            CreateMap<SubscriptionViewModel, Subscription>();
            CreateMap<NewsLetterViewModel, NewsLetter>();
        }
    }
}