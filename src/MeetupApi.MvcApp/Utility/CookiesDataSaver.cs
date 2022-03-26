using MeetupApi.Infrastructure;
using MeetupApi.MvcApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MeetupApi.MvcApp.Utility
{
    public class CookiesDataSaver
    {
        private static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;

        public static void SaveUserIntoCookies(AuthenticateResponse authenticationModel)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
            };

            string username;
            string userId;
            string userRole;
            bool isAuthenticated;

            GetUserClaims(authenticationModel, out userId, out username, out userRole, out isAuthenticated);

            _httpContext.Response.Cookies.Append(CookiesKeys.AccessToken, authenticationModel.Token, cookieOptions);
            _httpContext.Response.Cookies.Append(CookiesKeys.RefreshToken, authenticationModel.RefreshToken, cookieOptions);
            _httpContext.Response.Cookies.Append(CookiesKeys.UserId, userId, cookieOptions);
            _httpContext.Response.Cookies.Append(CookiesKeys.Username, username, cookieOptions);
            _httpContext.Response.Cookies.Append(CookiesKeys.UserRole, userRole, cookieOptions);
            _httpContext.Response.Cookies.Append(CookiesKeys.IsAuthenticated, isAuthenticated.ToString(), cookieOptions);
        }

        private static void GetUserClaims(AuthenticateResponse authenticationModel, out string userId, out string username, out string userRole, out bool isAuthenticated)
        {
            var stream = authenticationModel.Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            userId = tokenS!.Claims.First(claim => claim.Type == CommonClaim.UserIdClaim).Value;
            username = tokenS!.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            userRole = tokenS!.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            isAuthenticated = true;
        }
    }
}
