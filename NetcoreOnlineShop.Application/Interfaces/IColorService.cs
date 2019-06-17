using NetcoreOnlineShop.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IColorService
    {
        List<ColorViewModel> GetAll();

        ColorViewModel GetColorbyId(int id);
        void Save();
    }
}
