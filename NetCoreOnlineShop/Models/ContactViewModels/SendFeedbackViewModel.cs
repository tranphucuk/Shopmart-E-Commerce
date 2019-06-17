using NetcoreOnlineShop.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ContactViewModels
{
    public class SendFeedbackViewModel
    {
        public FeedbackViewModel Feedback { get; set; }

        public ContactViewModel Contact { get; set; } 
    }
}
