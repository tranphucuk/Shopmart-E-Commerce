using AutoMapper;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Page;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class PageService : IPageService
    {
        private IPageRepository _pageRepository;
        private IUnitOfWork _unitOfWork;
        public PageService(IPageRepository pageRepository, IUnitOfWork unitOfWork)
        {
            this._pageRepository = pageRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(PageViewModel pageVm)
        {
            var page = Mapper.Map<PageViewModel, Page>(pageVm);
            _pageRepository.Add(page);
        }

        public void Delete(int id)
        {
            _pageRepository.Remove(id);
        }

        public List<PageViewModel> GetAll()
        {
            var pages = _pageRepository.FindAll();
            var pageVms = Mapper.Map<List<Page>, List<PageViewModel>>(pages.ToList());
            return pageVms;
        }

        public PageResult<PageViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _pageRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            var pageVms = Mapper.Map<List<Page>, List<PageViewModel>>(query.ToList()).OrderByDescending(x => x.CreatedDate);
            var total = pageVms.Count();

            var result = pageVms.Skip((page - 1) * pageSize).Take(pageSize);
            var pageResult = new PageResult<PageViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = result.ToList(),
                RowCount = total
            };

            return pageResult;
        }

        public PageViewModel GetById(int id)
        {
            var page = _pageRepository.FindById(id);
            var pageVms = Mapper.Map<Page, PageViewModel>(page);
            return pageVms;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(PageViewModel pageVm)
        {
            var page = _pageRepository.FindById(pageVm.Id);
            if (page != null)
            {
                page.Name = pageVm.Name;
                page.Status = page.Status;
                page.Alias = pageVm.Alias;
                page.Content = pageVm.Content;
                page.Status = pageVm.Status;

                _pageRepository.Update(page);
            }
        }
    }
}
