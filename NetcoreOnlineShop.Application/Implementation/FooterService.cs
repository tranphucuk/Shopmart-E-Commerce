using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Footer;
using NetcoreOnlineShop.Application.ViewModels.Page;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class FooterService : IFooterService
    {
        private readonly IFooterRepository _footerRepository;
        private readonly IFooterPageRepository _footerPageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPageRepository _pageRepository;
        public FooterService(IFooterRepository footerRepository, IUnitOfWork unitOfWork,
            IFooterPageRepository footerPageRepository, IPageRepository pageRepository)
        {
            this._footerRepository = footerRepository;
            this._footerPageRepository = footerPageRepository;
            this._unitOfWork = unitOfWork;
            this._pageRepository = pageRepository;
        }

        public List<FooterViewModel> GetAll()
        {
            var footers = _footerRepository.FindAll();
            var footerVms = footers.ProjectTo<FooterViewModel>().OrderBy(x => x.Order);
            return footerVms.ToList();
        }

        public FooterViewModel GetById(int id)
        {
            var footer = _footerRepository.FindById(id);
            var footerVm = Mapper.Map<Footer, FooterViewModel>(footer);
            return footerVm;
        }
        public void Update(FooterViewModel footerVm)
        {
            var footer = _footerRepository.FindById(footerVm.Id);
            if (footer != null)
            {
                footer.Name = footerVm.Name;
                footer.Description = footer.Description;
                footer.Order = footerVm.Order;
                _footerRepository.Update(footer);
            }
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        //Footer Pages manager
        public List<FooterPageViewModel> GetPagesByFooterId(int footerId)
        {
            var footerPages = _footerPageRepository.FindAll(x => x.FooterId == footerId, x => x.Page);
            var footerPageVms = footerPages.ProjectTo<FooterPageViewModel>();
            return footerPageVms.ToList();
        }
        public bool AddPageToFooter(PageViewModel pageVm, int footerId)
        {
            var isExisted = _footerPageRepository.FindSingle(x => x.PageId == pageVm.Id && x.FooterId == footerId);
            if (isExisted == null)
            {
                var footer = _footerRepository.FindById(footerId);
                footer.FooterPages.Add(new FooterPage()
                {
                    FooterId = footerId,
                    PageId = pageVm.Id,
                    Status = Status.Active
                });
                return true;
            }
            return false;
        }

        public void RemovePageFooter(int footerPageId)
        {
            _footerPageRepository.Remove(footerPageId);
        }

        public List<PageViewModel> GetAllPagesByFooterId(int footerId)
        {
            var footerPages = _footerPageRepository.FindAll();
            var pages = _pageRepository.FindAll();

            var query = from fp in footerPages
                        join p in pages
                        on fp.PageId equals p.Id
                        where fp.FooterId == footerId
                        select p;
            var pageVms = Mapper.Map<List<Page>, List<PageViewModel>>(query.ToList());
            return pageVms;
        }
    }
}
