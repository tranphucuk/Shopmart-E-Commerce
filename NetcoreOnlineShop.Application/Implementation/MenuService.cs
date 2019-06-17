using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Menu;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
        {
            this._menuRepository = menuRepository;
            this._unitOfWork = unitOfWork;
        }

        public List<MenuViewModel> GetAll()
        {
            var menus = _menuRepository.FindAll(x => x.ParentId == 0);
            return menus.ProjectTo<MenuViewModel>().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}