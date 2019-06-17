using Microsoft.AspNetCore.Mvc;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResultBase pageResultBase)
        {
            return Task.FromResult((IViewComponentResult)View("Default", pageResultBase));
        }
    }
}
