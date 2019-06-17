using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Ticket;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Utilities.Constants;
using NetCoreOnlineShop.Extensions;
using NetCoreOnlineShop.Models.SupportTicketModels;
using NetCoreOnlineShop.Services;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers
{
    public class TicketController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBillService _billService;
        private readonly ISupportTicketService _supportTicketService;
        private readonly IEmailSender _emailSender;

        public TicketController(UserManager<AppUser> userManager, IBillService billService,
            ISupportTicketService supportTicketService, IEmailSender emailSender)
        {
            this._supportTicketService = supportTicketService;
            this._userManager = userManager;
            this._billService = billService;
            this._emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/user-ticket.html")]
        public async Task<IActionResult> OpenTicket()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;

            var ticketInfo = new TicketInfo();

            ticketInfo.BillIds = _billService.GetBillsByCustomerId(userId).Select(x => x.Id);

            return View(ticketInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/user-ticket.html")]
        public async Task<IActionResult> OpenTicket(SupportTicketViewModel ticketInfo)
        {
            if (!ModelState.IsValid || ticketInfo.BillId == 0)
            {
                ModelState.AddModelError(string.Empty, string.Empty);
                return RedirectToAction(nameof(TicketController.OpenTicket));
            }
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;

            var isTickerSent = _supportTicketService.isTicketexisted(userId, ticketInfo.BillId);
            if (isTickerSent)
            {
                return RedirectToAction(nameof(TicketController.TicketError), new { id = ticketInfo.BillId });
            }

            ticketInfo.UserId = userId;
            _supportTicketService.Add(ticketInfo);
            _supportTicketService.Save();

            // send email confirm to customer
            await _emailSender.SendTicket(ticketInfo.Email, ticketInfo.BillId);
            await _emailSender.SendTicket(CommonConstants.mailAdmin, ticketInfo.BillId);
            return RedirectToAction(nameof(TicketController.SendTicketSuccess), new { id = ticketInfo.BillId });
        }

        [HttpGet]
        [Route("/ticket-sent.html")]
        public IActionResult SendTicketSuccess(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(id);
        }

        [HttpGet]
        [Route("/ticket-error.html")]
        public IActionResult TicketError(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(id);
        }
    }
}