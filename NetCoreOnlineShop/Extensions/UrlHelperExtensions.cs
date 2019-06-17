using Microsoft.AspNetCore.Mvc;
using NetCoreOnlineShop.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, Guid userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, Guid userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string RequestRelativePath(ControllerBase controllerBase, string absolutePath)
        {
            var redundantPath = $"{controllerBase.Request.Scheme}://{controllerBase.Request.Host}/";
            var returnPath = absolutePath.Substring(redundantPath.Length);
            return returnPath;
        }
    }
}
