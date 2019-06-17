using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetcoreOnlineShop.Data.Enums
{
    public enum PaymentMethod
    {
        [DescriptionAttribute("Cash on delivery")]
        CashOnDelivery,
        [DescriptionAttribute("Online banking")]
        OnlineBanking,
        [DescriptionAttribute("Payment gateway")]
        PaymentGateway,
        [DescriptionAttribute("Visa")]
        Visa,
        [DescriptionAttribute("Master card")]
        MasterCard,
        [DescriptionAttribute("Paypal")]
        PayPal,
        [DescriptionAttribute("Atm")]
        Atm
    }
}
