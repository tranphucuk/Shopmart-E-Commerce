using NetcoreOnlineShop.Application.ViewModels.Page;
using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Footer
{
    public class FooterPageViewModel
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int FooterId { get; set; }
        public Status Status { get; set; }
        public PageViewModel Page { get; set; }

        public FooterViewModel Footer { get; set; }

    }
}
