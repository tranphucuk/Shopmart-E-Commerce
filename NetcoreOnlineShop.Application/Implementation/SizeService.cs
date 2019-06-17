using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;
        public SizeService(ISizeRepository sizeRepository)
        {
            this._sizeRepository = sizeRepository;
        }
        public List<SizeViewModel> GetAll()
        {
            var sizes = _sizeRepository.FindAll();
            var sizeVms = sizes.ProjectTo<SizeViewModel>(sizes).ToList();
            return sizeVms;
        }

        public SizeViewModel GetSizeById(int id)
        {
            var size = _sizeRepository.FindById(id);
            return Mapper.Map<Size, SizeViewModel>(size);
        }
    }
}
