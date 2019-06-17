using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Extensions
{
    public static class DetectDeviceExtension
    {
        public static string GetDeviceType(this string userAgent)
        {
            if (userAgent.Contains("Windows"))
            {
                return "Computer(Windows OS)";
            }
            else if (userAgent.Contains("Macintosh"))
            {
                return "Computer(Mac OS)";
            }
            else if (userAgent.Contains("Iphone"))
            {
                return "IPhone";
            }
            else if (userAgent.Contains("Ipad"))
            {
                return "IPad";
            }
            else if (userAgent.Contains("samsung"))
            {
                return "SamSung";
            }
            else if (userAgent.Contains("lg"))
            {
                return "LG";
            }
            else if (userAgent.Contains("sony"))
            {
                return "Sony";
            }
            else if (userAgent.Contains("ipod"))
            {
                return "IPod";
            }
            return "Unknown";
        }
    }
}
