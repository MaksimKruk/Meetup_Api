#nullable disable
namespace MeetupApi.MvcApp.Models
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
