using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Extensions
{
    public static class GetEnumDescription
    {
        public static string GetDescription<TEnum>(this Enum source)
        {
            var description = string.Empty;
            var type = source.GetType();
            var members = type.GetMember(source.ToString());
            var descriptionAttributes = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptionAttributes.Length > 0)
            {
                description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
            }
            return description;
        }
    }
}
