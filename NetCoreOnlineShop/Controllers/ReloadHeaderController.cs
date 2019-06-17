using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreOnlineShop.Controllers
{
    public class ReloadHeaderController : Controller
    {
        public IActionResult HeaderDetail()
        {
            return ViewComponent("HeaderDetail");
        }
    }
}