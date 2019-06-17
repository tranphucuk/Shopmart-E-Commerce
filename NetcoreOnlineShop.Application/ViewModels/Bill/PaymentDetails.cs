using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Bill
{
    public class PaymentDetails
    {
        public PaymentDetails()
        {
            Payments = new PaymentNumber();
        }
        public PaymentNumber Payments { get; set; }

        public int Total { get; set; }
    }
}
