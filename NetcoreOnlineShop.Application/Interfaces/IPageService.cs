using NetcoreOnlineShop.Application.ViewModels.Page;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IPageService
    {
        List<PageViewModel> GetAll();

        PageResult<PageViewModel> GetAllPaging(string keyword, int page, int pageSize);

        void Add(PageViewModel pageVm);

        void Update(PageViewModel pageVm);

        void Delete(int id);

        PageViewModel GetById(int id);

        void Save();
    }
}
