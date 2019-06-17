using NetcoreOnlineShop.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface ISizeService
    {
        List<SizeViewModel> GetAll();

        SizeViewModel GetSizeById(int id);
    }
}
