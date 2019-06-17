using NetcoreOnlineShop.Application.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.SupportTicketModels
{
    public class TicketInfo
    {
        public SupportTicketViewModel Ticket { get; set; }

        public IEnumerable<int> BillIds { get; set; }
    }
}
