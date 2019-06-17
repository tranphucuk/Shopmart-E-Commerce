using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetcoreOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Helpers
{
    public class CustomClaimPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        UserManager<AppUser> _userManager;

        public CustomClaimPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            this._userManager = userManager;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principals = await base.CreateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            ((ClaimsIdentity)principals.Identity).AddClaims(new[]
            {
                new Claim("Email",user.Email),
                new Claim("FullName",user.FullName),
                new Claim("Roles",string.Join(";",roles)),
                new Claim("Avatar",user.Avatar ?? string.Empty),
            });

            return principals;
        }
    }
}
