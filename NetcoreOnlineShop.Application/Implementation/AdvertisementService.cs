using AutoMapper;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Ads;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IAdvertisementPageNameRepository _advertisementPageNameRepository;
        private readonly IAdvertisementPageRepository _advertisementPageRepository;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdvertisementService(IAdvertisementRepository advertisementRepository, IUnitOfWork unitOfWork,
            IAdvertisementPageNameRepository advertisementPageNameRepository, IAdvertisementPageRepository advertisementPageRepository)
        {
            this._advertisementPageNameRepository = advertisementPageNameRepository;
            this._advertisementPageRepository = advertisementPageRepository;
            this._advertisementRepository = advertisementRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(AdvertisementViewModel adv)
        {
            var advertisement = Mapper.Map<AdvertisementViewModel, Advertisement>(adv);
            _advertisementRepository.Add(advertisement);
        }

        public bool AddPage(AdvertisementPageNameViewModel pageName)
        {
            var isExisted = _advertisementPageNameRepository.FindSingle(x => x.Name == pageName.Name);
            if (isExisted == null)
            {
                var page = Mapper.Map<AdvertisementPageNameViewModel, AdvertisementPageName>(pageName);
                _advertisementPageNameRepository.Add(page);
                return true;
            }
            return false;
        }

        public void Delete(int id)
        {
            _advertisementRepository.Remove(id);
        }

        public List<AdvertisementViewModel> GetAll()
        {
            var advs = _advertisementRepository.FindAll();
            var advVms = Mapper.Map<List<Advertisement>, List<AdvertisementViewModel>>(advs.ToList());
            return advVms;
        }

        public AdvertisementViewModel GetById(int id)
        {
            var adv = _advertisementRepository.FindById(id);
            var advertisement = Mapper.Map<Advertisement, AdvertisementViewModel>(adv);
            return advertisement;
        }

        public List<AdvertisementPageNameViewModel> LoadPage()
        {
            var pages = _advertisementPageNameRepository.FindAll();
            var pageVms = Mapper.Map<List<AdvertisementPageName>, List<AdvertisementPageNameViewModel>>(pages.ToList());
            return pageVms;
        }

        public void AddAdsPage(AdvertisementPageViewModel adsPage)
        {
            var advertisement = _advertisementRepository.FindById(adsPage.AdvertisementId);
            if (advertisement != null)
            {
                var adsPageModel = Mapper.Map<AdvertisementPageViewModel, AdvertisementPage>(adsPage);
                advertisement.AdvertisementPages.Add(adsPageModel);
            }
        }
        public void UpdateAdsPage(AdvertisementPageViewModel adsPage)
        {
            var adsPageModel = _advertisementPageRepository.FindById(adsPage.Id);
            if (adsPageModel != null)
            {
                adsPageModel.AdvertisementId = adsPage.AdvertisementId;
                adsPageModel.AdvertisementPageNameId = adsPage.AdvertisementPageNameId;
                adsPageModel.Position = adsPage.Position;

                _advertisementPageRepository.Update(adsPageModel);
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(AdvertisementViewModel adv)
        {
            var advertisement = _advertisementRepository.FindById(adv.Id);
            if (advertisement != null)
            {
                advertisement.Description = adv.Description;
                advertisement.Height = adv.Height;
                advertisement.Image = adv.Image;
                advertisement.Name = adv.Name;
                advertisement.SortOrder = adv.SortOrder;
                advertisement.Status = adv.Status;
                advertisement.Url = adv.Url;
                advertisement.Width = adv.Width;
            }
            _advertisementRepository.Update(advertisement);
        }

        public List<AdvertisementPageViewModel> GetListAdsPage(int advId)
        {
            var adsPage = _advertisementPageRepository.FindAll(x => x.AdvertisementId == advId);
            var adsPageVms = Mapper.Map<List<AdvertisementPage>, List<AdvertisementPageViewModel>>(adsPage.ToList());

            return adsPageVms;
        }

        public void RemoveAdsPage(List<AdvertisementPageViewModel> adsPages, int adsId)
        {
            var allAdsPages = _advertisementPageRepository.FindAll(x => x.AdvertisementId == adsId);
            var adsPagesModels = Mapper.Map<List<AdvertisementPageViewModel>, List<AdvertisementPage>>(adsPages);

            var listAdsPageRemove = allAdsPages.Where(x => !adsPagesModels.Contains(x)).ToList();

            _advertisementPageRepository.RemoveMultiple(listAdsPageRemove);
        }

        public List<AdvertisementViewModel> GetClientSideAds(string pageName)
        {
            var listAds = _advertisementRepository.FindAll();
            var listAdsPages = _advertisementPageRepository.FindAll();
            var pageNameId = _advertisementPageNameRepository.FindSingle(x => x.Name == pageName).Id;

            var query = from ads in listAds
                        join adPage in listAdsPages
                        on ads.Id equals adPage.AdvertisementId
                        where adPage.AdvertisementPageNameId == pageNameId
                        select ads;
            var listAdsViewModels = Mapper.Map<List<Advertisement>, List<AdvertisementViewModel>>(query.ToList());
            return listAdsViewModels;
        }
    }
}
