using MeetupApi.MvcApp.Models;
using MeetupApi.MvcApp.Utility.HttpClients;
using System.IdentityModel.Tokens.Jwt;

#nullable disable
namespace MeetupApi.MvcApp.Utility
{
    public class CheckTokenData
    {
        private static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;

        private readonly IdentityApiClient _identityApiClient;

        public CheckTokenData(IdentityApiClient identityApiClient)
        {
            _identityApiClient = identityApiClient;
        }

        public async Task RefreshAsync()
        {
            if (!string.IsNullOrEmpty(_httpContext.Request.Cookies[CookiesKeys.AccessToken]) &
                !string.IsNullOrEmpty(_httpContext.Request.Cookies[CookiesKeys.RefreshToken]))
            {
                var accessToken = _httpContext.Request.Cookies[CookiesKeys.AccessToken];
                var refreshToken = _httpContext.Request.Cookies[CookiesKeys.RefreshToken];

                TokenModel tokenModel = new TokenModel { AccessToken = accessToken, RefreshToken = refreshToken };

                TokenModel tokenInfo = null;
                AuthenticateResponse authenticateResponse = null;

                using (var result = await _identityApiClient.Client.PostAsJsonAsync("refresh-token", tokenModel))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        tokenInfo = await result.Content.ReadFromJsonAsync<TokenModel>();
                        authenticateResponse = new AuthenticateResponse() { Token = tokenInfo.AccessToken, RefreshToken = tokenInfo.RefreshToken };
                        CookiesDataSaver.SaveUserIntoCookies(authenticateResponse);
                    }
                }
            }
        }

        public static bool _isInvalid(string token)
        {
            JwtSecurityToken jwtToken = null;
            if (!string.IsNullOrEmpty(token))
            {
                jwtToken = new JwtSecurityToken(token);
            }
            return (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
        }
    }
}
