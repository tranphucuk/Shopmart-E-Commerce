using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Utilities.Constants;
using NetCoreOnlineShop.Extensions;
using NetCoreOnlineShop.Models.ContactViewModels;
using NetCoreOnlineShop.Services;
using static NetcoreOnlineShop.Utilities.Constants.CommonConstants;

namespace NetCoreOnlineShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IFeedbackService _feedbackService;
        private readonly IEmailSender _emailSender;
        public ContactController(IContactService contactService, IFeedbackService feedbackService, IEmailSender emailSender)
        {
            this._contactService = contactService;
            this._feedbackService = feedbackService;
            this._emailSender = emailSender;
        }

        [HttpGet]
        [Route("/contact-us.html")]
        public IActionResult Index()
        {
            var contact = _contactService.LoadContactDetail();
            var feedbackVm = new SendFeedbackViewModel();
            feedbackVm.Contact = contact;

            var flag = HttpContext.Session.Get<FlagIndicate>(Flag.Contact);
            if (flag != null)
            {
                TempData["StatusMessage"] = flag.Content;
                HttpContext.Session.Remove(Flag.Contact);
            }
            return View(feedbackVm);
        }

        [HttpPost]
        [Route("/contact-us.html")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(FeedbackViewModel feedbackViewModel)
        {
            SendFeedbackViewModel details = new SendFeedbackViewModel();
            var contact = _contactService.LoadContactDetail();
            details.Contact = contact;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, string.Empty);
                return View("Index", details);
            }
            _feedbackService.AddFeedback(feedbackViewModel);
            _feedbackService.Save();

            var flag = HttpContext.Session.Get<FlagIndicate>(Flag.Contact);
            if (flag == null)
            {
                flag = new FlagIndicate();
                flag.Content = Flag.Contact;
                HttpContext.Session.Set<FlagIndicate>(Flag.Contact, flag);
            }

            await _emailSender.SendFeedback(CommonConstants.mailAdmin, feedbackViewModel);
            return RedirectToAction("Index", "Contact");
        }
    }
}