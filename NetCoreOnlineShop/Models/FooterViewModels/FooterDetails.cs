using NetcoreOnlineShop.Application.ViewModels.Footer;
using NetcoreOnlineShop.Application.ViewModels.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.FooterViewModels
{
    public class FooterDetails
    {
        public FooterViewModel FooterViewModel { get; set; }

        public List<PageViewModel> PageViewModels { get; set; }
    }
}
