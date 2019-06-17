using NetcoreOnlineShop.Application.ViewModels.Ads;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IAdvertisementService
    {
        List<AdvertisementViewModel> GetAll();

        void Add(AdvertisementViewModel adv);

        void Update(AdvertisementViewModel adv);

        AdvertisementViewModel GetById(int id);

        bool AddPage(AdvertisementPageNameViewModel pageName);

        List<AdvertisementPageNameViewModel> LoadPage();

        void AddAdsPage(AdvertisementPageViewModel adsPage);
        void UpdateAdsPage(AdvertisementPageViewModel adsPage);
        void RemoveAdsPage(List<AdvertisementPageViewModel> adsPages, int adsId);

        void Delete(int id);
        List<AdvertisementPageViewModel> GetListAdsPage(int advId);
        void Save();

        // Client ads
        List<AdvertisementViewModel> GetClientSideAds(string pageName);
    }
}
