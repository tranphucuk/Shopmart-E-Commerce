using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.Interfaces
{
    public interface IDateTracking
    {
        DateTime CreatedDate { get; set; }

        DateTime ModifiedDate { get; set; }
    }
}
