using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Utilities.Exceptions
{
    public class BadException : Exception
    {
        public BadException(string mess) : base(mess)
        {

        }

        public BadException(string mess, Exception exception) : base(mess, exception)
        {

        }
    }
}
