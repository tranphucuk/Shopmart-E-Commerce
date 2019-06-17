using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetcoreOnlineShop.Data.Enums
{
    public enum BillStatus
    {
        [DescriptionAttribute("New")]
        New,
        [DescriptionAttribute("In progress")]
        InProgress,
        [DescriptionAttribute("Returned")]
        Returned,
        [DescriptionAttribute("Cancelled")]
        Cancelled,
        [DescriptionAttribute("Completed")]
        Completed
    }
}
