using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Slide;
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
    public class SlideService : ISlideService
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SlideService(ISlideRepository slideRepository, IUnitOfWork unitOfWork)
        {
            this._slideRepository = slideRepository;
            this._unitOfWork = unitOfWork;
        }
        public void AddSlide(SlideViewModel slideViewModel)
        {
            var slide = Mapper.Map<SlideViewModel, Slide>(slideViewModel);
            _slideRepository.Add(slide);
        }

        public void DeleteSlide(int id)
        {
            _slideRepository.Remove(id);
        }

        public List<SlideViewModel> GetAll()
        {
            var slides = _slideRepository.FindAll();
            var slideViewModels = slides.ProjectTo<SlideViewModel>().ToList();

            return slideViewModels;
        }

        public PageResult<SlideViewModel> GetAllPaging(int page, int pageSize, string keyword)
        {
            var query = _slideRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Text.Contains(keyword) || x.Url.Contains(keyword));
            }

            var total = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var pageResult = new PageResult<SlideViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = query.ProjectTo<SlideViewModel>().ToList(),
                RowCount = total
            };

            return pageResult;
        }

        public SlideViewModel GetById(int id)
        {
            var slide = _slideRepository.FindById(id);
            return Mapper.Map<Slide, SlideViewModel>(slide);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateSlide(SlideViewModel slideViewModel)
        {
            var slide = Mapper.Map<SlideViewModel, Slide>(slideViewModel);
            _slideRepository.Update(slide);
        }
    }
}
