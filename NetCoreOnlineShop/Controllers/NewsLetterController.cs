using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.NewsLetter;
using NetcoreOnlineShop.Data.Entities;

namespace NetCoreOnlineShop.Controllers
{
    public class NewsLetterController : Controller
    {
        private readonly INewsLetterService _newsLetter;
        public NewsLetterController(INewsLetterService newsLetter)
        {
            this._newsLetter = newsLetter;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubscribeEmail(SubscriptionViewModel subscription)
        {
            if (!ModelState.IsValid)
            {
                return new OkObjectResult(false);
            }

            _newsLetter.SubScribeEmail(subscription);
            _newsLetter.Save();

            return new OkObjectResult(true);
        }
    }
}