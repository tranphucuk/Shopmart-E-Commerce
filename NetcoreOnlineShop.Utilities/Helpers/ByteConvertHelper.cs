using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Utilities.Helpers
{
    public static class ByteConvertHelper
    {
        public static byte[] Base64ToImage(string source)
        {
            string base64 = source.Substring(source.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            return Convert.FromBase64String(base64);
        }

        public static decimal FromByteToMegabyte(byte[] valueInput)
        {
            var mega = (decimal)valueInput.Length / (1024 *1024);
            return mega;
        }
    }
}
