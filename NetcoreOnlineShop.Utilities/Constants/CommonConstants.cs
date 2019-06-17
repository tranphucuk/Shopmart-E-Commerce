using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Utilities.Constants
{
    public static class CommonConstants
    {
        public static string productTag = "Product";
        public static string postTag = "Post";
        public static string blogTag = "Blog";

        public static string deleteMethod = "delete";
        public static string addMethod = "add";
        public static string updateMethod = "update";

        public static string exceededSize = "File size is over the maximum size";
        public static string commonError = "An error has occurred.";
        public static string notFound = "Not found";

        public static decimal maxSizeImg = 3;

        public static string CartSession = "CartSession";

        public class Flag
        {
            public const string Contact = "Success";
        }
        public class ProductType
        {
            public const string NewArrival = "New arrival";
            public const string SpeicalOffer = "Speical offer";
            public const string BestSeller = "Best seller";
        }

        public static string mailAdmin = "tranphucggg@gmail.com";
        public class AppRole
        {
            public const string AdminRole = "Admin";
            public const string CustomerRole = "Customer";
            public const string StaffRole = "Staff";
        }

        public class SortType
        {
            public const string Price = "price";
            public const string Name = "name";
        }

        public class UserClaims
        {
            public const string Roles = "Roles";
        }

        public class HomeMeta
        {
            public const string Title = "Shop Mart";
            public const string Keyword = "Shop Mart - Save your money";
            public const string Description = "Shop Mart for appliances, tools, clothing, mattresses & more.";
        }

        public class AdsPages
        {
            public const string Home = "Home";
            public const string Blog = "Blog";
            public const string Category = "Category";
        }

        public class DashboardUrl
        {
            public string Url { get; set; }

            public string Name { get; set; }
        }

        public static List<DashboardUrl> listLinks = new List<DashboardUrl>() // User dashboard links
        {
            new DashboardUrl(){Name = "Purchase History",Url="/order-history.html"},
            new DashboardUrl(){Name = "My Wishlist",Url="/wishlist.html"},
            new DashboardUrl(){Name = "Account Information",Url="/user-dashboard.html"},
            new DashboardUrl(){Name = "Announcement",Url="/user-announcement.html"},
            new DashboardUrl(){Name = "Support Ticket",Url="/user-ticket.html"},
        };
    }
}
