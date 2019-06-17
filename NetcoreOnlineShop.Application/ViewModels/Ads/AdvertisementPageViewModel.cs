using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Ads
{
    public class AdvertisementPageViewModel
    {
        public int Id { get; set; }
        public Position Position { get; set; }

        public int AdvertisementId { get; set; }

        public int AdvertisementPageNameId { get; set; }

        public Advertisement Advertisement { get; set; }

        public AdvertisementPageNameViewModel AdvertisementPageName { get; set; }
    }
}
