using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ColorService(IColorRepository colorRepository, IUnitOfWork unitOfWork)
        {
            this._colorRepository = colorRepository;
            this._unitOfWork = unitOfWork;
        }
        public List<ColorViewModel> GetAll()
        {
            var colors = _colorRepository.FindAll();
            var colorVms = colors.ProjectTo<ColorViewModel>(colors).ToList();
            return colorVms;
        }

        public ColorViewModel GetColorbyId(int id)
        {
            var color = _colorRepository.FindById(id);
            return Mapper.Map<Color, ColorViewModel>(color);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
