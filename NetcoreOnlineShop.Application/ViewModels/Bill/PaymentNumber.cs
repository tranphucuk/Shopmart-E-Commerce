using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Bill
{
    public class PaymentNumber
    {
        [DisplayName("Cash on delivery")]
        public int CashOnDelivery { get; set; }

        [DisplayName("Online banking")]
        public int OnlineBanking { get; set; }

        [DisplayName("Payment Gateway")]
        public int PaymentGateway { get; set; }

        [DisplayName("Visa")]
        public int Visa { get; set; }

        [DisplayName("MasterCard")]
        public int MasterCard { get; set; }

        [DisplayName("PayPal")]
        public int PayPal { get; set; }

        [DisplayName("Atm")]
        public int Atm { get; set; }
    }
}
