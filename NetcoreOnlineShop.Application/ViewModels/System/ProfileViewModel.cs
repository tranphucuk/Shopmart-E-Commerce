using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.System
{
    public class ProfileViewModel
    {
        public AppUserViewModel User { get; set; }

        public PageResult<AppUserActivityViewModel> Activities { get; set; } 
    }
}
