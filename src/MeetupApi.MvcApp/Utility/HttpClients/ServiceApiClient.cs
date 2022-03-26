#nullable disable

namespace MeetupApi.MvcApp.Utility.HttpClients
{
    public class ServiceApiClient
    {
        public HttpClient Client { get; private set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            httpClient.BaseAddress = new Uri("http://localhost:26512/api/Events/");
            string token = _httpContextAccessor.HttpContext.Request.Cookies[CookiesKeys.AccessToken];
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Client = httpClient;
        }
    }
}
