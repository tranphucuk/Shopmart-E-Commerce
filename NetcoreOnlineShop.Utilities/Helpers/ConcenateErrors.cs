using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Utilities.Helpers
{
    public static class ConcenateErrors
    {
        public static string Error(IEnumerable<IdentityError> errors)
        {
            var error = string.Empty;
            foreach (var item in errors)
            {
                error += $"{item.Code}: {item.Description}\n";
            }
            return error;
        }
    }
}
