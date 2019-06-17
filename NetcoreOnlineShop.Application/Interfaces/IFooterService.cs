using NetcoreOnlineShop.Application.ViewModels.Footer;
using NetcoreOnlineShop.Application.ViewModels.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IFooterService
    {
        List<FooterViewModel> GetAll();

        void Update(FooterViewModel footerVm);

        FooterViewModel GetById(int id);

        bool AddPageToFooter(PageViewModel pageVm, int footerId);

        List<FooterPageViewModel> GetPagesByFooterId(int footerId);

        void RemovePageFooter(int footerPageId);

        List<PageViewModel> GetAllPagesByFooterId(int footerId);
        void Save();
    }
}
