using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Utilities.Constants;
using NetCoreOnlineShop.Extensions;
using NetCoreOnlineShop.Models;
using NetCoreOnlineShop.Models.IdentityViewModel;
using NetCoreOnlineShop.Services;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(UserManager<AppUser> userManage, IEmailSender emailSender, SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger, IUserService userService)
        {
            this._userManager = userManage;
            this._emailSender = emailSender;
            this._signInManager = signInManager;
            this._userService = userService;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("register.html")]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateRecaptcha]
        [ValidateAntiForgeryToken]
        [Route("register.html")]
        public async Task<IActionResult> Register(RegisterViewModel userInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(userInfo);
            }
            var user = new AppUser()
            {
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                Address = userInfo.Address,
                FullName = userInfo.FullName,
                PhoneNumber = userInfo.PhoneNumber,
                BirthDay = userInfo.BirthDay,
                CreatedDate = DateTime.Now,
                Status = Status.InActive
            };

            var result = await _userManager.CreateAsync(user, userInfo.Password);
            if (result.Succeeded)
            {
                var isSuccess = await _userManager.AddToRoleAsync(user, CommonConstants.AppRole.CustomerRole);
                if (!isSuccess.Succeeded)
                {
                    _logger.LogError("Add user to role 'Customer' error", new object[] { user.UserName, user.Id, user.Email, user.CreatedDate });
                }
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);
            }
            else
            {
                ModelState.AddModelError(result.Errors.First().Code, result.Errors.First().Description);
                return View();
            }
            return View("ConfirmEmail");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (returnUrl == null)
            {
                ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
            }
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, IdentityConstants.ExternalScheme);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var userStatus = await _userManager.FindByNameAsync(login.Username);
                if (userStatus != null && userStatus.Status == Status.InActive)
                {
                    ViewData["ReturnUrl"] = "/login.html";
                    ModelState.AddModelError(string.Empty, "Your account is not verified. Plese confirm your email.");
                    return View();
                }
                var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded && userStatus.Status == Status.Active)
                {
                    string page = "https://ident.me/";//"https://ip.seeip.org/"
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.GetAsync(page);
                        if (response != null)
                        {
                            var content = response.Content.ReadAsStringAsync();
                            var userActivity = new AppUserActivityViewModel();
                            userActivity.IPAddress = content.Result.Trim();
                            var userAgent = Request.Headers["User-Agent"];
                            userActivity.Device = DetectDeviceExtension.GetDeviceType(userAgent);
                            userActivity.UserId = userStatus.Id;
                            userActivity.LastSession = DateTime.Now;
                            userActivity.Username = userStatus.UserName;
                            _userService.AddActivity(userActivity);
                            _userService.Save();
                        }
                    }
                    if (returnUrl == null || returnUrl.Contains("login") || returnUrl.Contains("ResetPasswordConfirmation") || returnUrl.Contains("ConfirmEmail"))
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    // Convert returnUrl
                    if (!Url.IsLocalUrl(returnUrl))
                    {
                        var redundantPath = $"{Request.Scheme}://{Request.Host}";
                        var returnPath = returnUrl.Substring(redundantPath.Length);
                        returnUrl = returnPath;
                    }
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    var endTime = await _userManager.GetLockoutEndDateAsync(userStatus);
                    return View("LockoutAccount", endTime);
                }
                else
                {
                    ViewData["ReturnUrl"] = "/login.html";
                    if (userStatus != null)
                    {
                        ModelState.AddModelError(string.Empty, $"Invalid login credentials. Attempts remain: {5 - userStatus.AccessFailedCount}");
                        return View();
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
            // If we got this far, something failed, redisplay form
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            var cookies = HttpContext.Request.Cookies;
            if (cookies.Count() > 0)
            {
                foreach (var cookie in cookies)
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(AccountController.Index), "Account");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return View("ErrorConfirmEmail");
                //throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            // change user status to Active if email is confirmed
            var userInfo = await _userManager.FindByIdAsync(userId);
            if (userInfo != null)
            {
                userInfo.Status = Status.Active;
                var updateSuccess = await _userManager.UpdateAsync(userInfo);
                if (!updateSuccess.Succeeded)
                {
                    return View("ErrorConfirmEmail");
                }
            }
            return View("ConfirmedEmail");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("forgot-password.html", Name = "Forgot Password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateRecaptcha]
        [ValidateAntiForgeryToken]
        [Route("forgot-password.html", Name = "Forgot Password")]
        public async Task<IActionResult> ForgotPassword(EmailViewModel emailAddress)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(emailAddress.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ModelState.AddModelError(string.Empty, "Email is not existed");
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailResetPasswordAsync(emailAddress.Email, callbackUrl);

                return View("ResetPasswordEmail");
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("reset-password.html", Name = "Reset Password")]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return View("UserNotFound");
            }
            else
            {
                var ResetPassword = new ResetPasswordViewModel
                {
                    Code = code
                };
                return View(ResetPassword);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateRecaptcha]
        [ValidateAntiForgeryToken]
        [Route("reset-password.html", Name = "Reset Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword, string userId = null)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Code, resetPassword.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View("ResetPasswordConfirmed");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (result.IsLockedOut)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email);
                var endTime = await _userManager.GetLockoutEndDateAsync(user);
                return View("LockoutAccount", endTime);
            }
            else
            {
                ViewData["LoginProvider"] = info.LoginProvider;
                return View("ExternalLogin", new ExternalLoginViewModel());
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateRecaptcha]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel externalLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, string.Empty);
                return View("ExternalLogin", externalLoginViewModel);
            }
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                //ErrorMessage = "Error loading external login information during confirmation.";
                return View(nameof(AccountController.Login));
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    Address = externalLoginViewModel.Address,
                    BirthDay = externalLoginViewModel.BirthDay,
                    FullName = externalLoginViewModel.FullName,
                    PhoneNumber = externalLoginViewModel.PhoneNumber,
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(nameof(ExternalLogin), externalLoginViewModel);
        }

        [HttpGet]
        [Route("/user-dashboard.html")]
        public async Task<IActionResult> ChangePassword(Guid userId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(AccountController.Login));
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userDashboard = new AccountDashboard()
            {
                Address = user.Address,
                BirthDay = user.BirthDay,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return View(userDashboard);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("/user-dashboard.html")]
        public async Task<IActionResult> ChangePassword(AccountDashboard userInfo)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, string.Empty);
                return View(userInfo);
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(AccountController.Login));
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var isExisted = await _signInManager.PasswordSignInAsync(user, userInfo.OldPassword, isPersistent: false, lockoutOnFailure: false);
                if (isExisted.Succeeded)
                {
                    user.Address = userInfo.Address;
                    user.PhoneNumber = userInfo.PhoneNumber;
                    user.BirthDay = userInfo.BirthDay;
                    user.FullName = userInfo.FullName;

                    var IsUpdated = await _userManager.UpdateAsync(user);
                    if (!IsUpdated.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, string.Empty);
                        return View(userInfo);
                    }

                    if (string.IsNullOrEmpty(userInfo.NewPassword))
                    {
                        return RedirectToAction(nameof(AccountController.ChangePassword));
                    }

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, userInfo.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(AccountController.ChangePassword));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, string.Empty);
                        return View(userInfo);
                    }
                }
            }
            return RedirectToAction(nameof(AccountController.Login));
        }
    }
}