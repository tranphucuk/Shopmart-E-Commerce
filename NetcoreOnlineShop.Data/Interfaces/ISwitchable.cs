using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}
