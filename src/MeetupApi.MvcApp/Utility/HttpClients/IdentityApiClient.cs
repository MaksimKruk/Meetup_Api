#nullable disable
using System.Security.Claims;

namespace MeetupApi.MvcApp.Utility.HttpClients
{
    public class IdentityApiClient
    {
        public HttpClient Client { get; private set; }

        public IdentityApiClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://localhost:5792/api/Authenticate/");
            Client = httpClient;
        }
    }
}
