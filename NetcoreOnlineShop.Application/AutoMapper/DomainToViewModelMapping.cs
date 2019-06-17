using AutoMapper;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Ads;
using NetcoreOnlineShop.Application.ViewModels.Announcement;
using NetcoreOnlineShop.Application.ViewModels.Blog;
using NetcoreOnlineShop.Application.ViewModels.Footer;
using NetcoreOnlineShop.Application.ViewModels.Menu;
using NetcoreOnlineShop.Application.ViewModels.NewsLetter;
using NetcoreOnlineShop.Application.ViewModels.Page;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Application.ViewModels.Slide;
using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Application.ViewModels.Ticket;
using NetcoreOnlineShop.Data.Entities;

namespace NetcoreOnlineShop.Application.AutoMapper
{
    public class DomainToViewModelMapping : Profile
    {
        public DomainToViewModelMapping()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<Color, ColorViewModel>();
            CreateMap<Size, SizeViewModel>();
            CreateMap<ProductQuantity, ProductQuantityViewModel>();
            CreateMap<ProductImage, ProductImageViewModel>();
            CreateMap<WholePrice, WholePriceViewModel>();
            CreateMap<Menu, MenuViewModel>();
            CreateMap<Slide, SlideViewModel>();
            CreateMap<Feedback, FeedbackViewModel>();
            CreateMap<Blog, BlogViewModel>();
            CreateMap<BlogImage, BlogImageViewModel>();
            CreateMap<Tag, TagViewModel>();
            CreateMap<BlogTag, BlogTagViewModel>();
            CreateMap<Contact, ContactViewModel>();
            CreateMap<Page, PageViewModel>();
            CreateMap<Footer, FooterViewModel>();
            CreateMap<FooterPage, FooterPageViewModel>();
            CreateMap<AppUserActivity, AppUserActivityViewModel>();
            CreateMap<Advertisement, AdvertisementViewModel>();
            CreateMap<AdvertisementPageName, AdvertisementPageNameViewModel>();
            CreateMap<AdvertisementPage, AdvertisementPageViewModel>();
            CreateMap<Announcement, AnnouncementViewModel>();
            CreateMap<SupportTicket, SupportTicketViewModel>();
            CreateMap<Subscription, SubscriptionViewModel>();
            CreateMap<NewsLetter, NewsLetterViewModel>();
        }
    }
}