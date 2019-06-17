using NetcoreOnlineShop.Application.ViewModels.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IMenuService
    {
        List<MenuViewModel> GetAll();
        void Save();
    }
}
