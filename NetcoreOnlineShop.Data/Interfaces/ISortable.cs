using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.Interfaces
{
    public interface ISortable
    {
        int SortOrder { get; set; }
    }
}
