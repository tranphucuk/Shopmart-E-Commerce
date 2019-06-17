using NetcoreOnlineShop.Application.ViewModels.Slide;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface ISlideService
    {
        List<SlideViewModel> GetAll();
        PageResult<SlideViewModel> GetAllPaging(int page, int pageSize, string keyword);

        SlideViewModel GetById(int id);
        void AddSlide(SlideViewModel slideViewModel);
        void UpdateSlide(SlideViewModel slideViewModel);

        void DeleteSlide(int id);

        void Save();
    }
}
