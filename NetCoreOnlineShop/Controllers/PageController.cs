using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetcoreOnlineShop.Application.Interfaces;

namespace NetCoreOnlineShop.Controllers
{
    public class PageController : Controller
    {
        private IPageService _pageService;
        public PageController(IPageService pageService)
        {
            this._pageService = pageService;
        }
        [Route("/page-{alias}-{id}.html")]
        public IActionResult Index(string alias, int id)
        {
            var pageVm = _pageService.GetById(id);
            return View(pageVm);
        }
    }
}