using MeetupApi.Infrastructure;
using MeetupApi.MvcApp.Models;
using MeetupApi.MvcApp.Utility;
using MeetupApi.MvcApp.Utility.HttpClients;
using MeetupApi.MvcApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MeetupApi.MvcApp.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Custom HttpClient implementation.
        /// </summary>
        private readonly IdentityApiClient _identityApiClient;

        /// <summary>
        /// Class constructor. 
        /// </summary>
        public AccountController(IdentityApiClient identityApiClient)
        {
            _identityApiClient = identityApiClient;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var result = await _identityApiClient.Client.PostAsJsonAsync("register", registerViewModel))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        var errorMessage = await result.Content.ReadFromJsonAsync<Response>();
                        ModelState.AddModelError(string.Empty, errorMessage!.Message!);
                    }
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            if (Convert.ToBoolean(HttpContext.Request.Cookies[CookiesKeys.IsAuthenticated]))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var result = await _identityApiClient.Client.PostAsJsonAsync("login", loginViewModel))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var userProfileInfo = await result.Content.ReadFromJsonAsync<AuthenticateResponse>();
                        //SaveIntoCookies(userProfileInfo!);
                        CookiesDataSaver.SaveUserIntoCookies(userProfileInfo!);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var errorMessage = await result.Content.ReadFromJsonAsync<Response>();
                        ModelState.AddModelError(string.Empty, errorMessage!.Message!);
                    }
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete(CookiesKeys.AccessToken);
            HttpContext.Response.Cookies.Delete(CookiesKeys.RefreshToken);
            HttpContext.Response.Cookies.Delete(CookiesKeys.UserId);
            HttpContext.Response.Cookies.Delete(CookiesKeys.Username);
            HttpContext.Response.Cookies.Delete(CookiesKeys.UserRole);
            HttpContext.Response.Cookies.Delete(CookiesKeys.IsAuthenticated);
            return Redirect("~/Home/Index");
        }
    }
}
