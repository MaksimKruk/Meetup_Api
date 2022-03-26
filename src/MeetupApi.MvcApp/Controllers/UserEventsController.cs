using MeetupApi.Infrastructure;
using MeetupApi.MvcApp.Utility;
using MeetupApi.MvcApp.Utility.HttpClients;
using MeetupApi.MvcApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MeetupApi.MvcApp.Controllers
{
    public class UserEventsController : Controller
    {
        private readonly ServiceApiClient _serviceApiClient;

        public UserEventsController(ServiceApiClient serviceApiClient)
        {
            _serviceApiClient = serviceApiClient;
        }

        public async Task<IActionResult> Index()
        {
            string? userId = HttpContext.Request.Cookies[CookiesKeys.UserId];
            if (string.IsNullOrEmpty(userId)) userId = "null";

            IEnumerable<EventViewModel>? model = null;
            using (var result = await _serviceApiClient.Client.GetAsync("all/special/" + userId))
            {
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsAsync<IList<EventViewModel>>();
                    model = readTask;
                }
                if(model == null) return NotFound();
            }
            return View("Index", model);
        }

        public async Task<ActionResult> Details(int id)
        {
            return await GetById(id);
        }

        public async Task<ActionResult> GetById(int id)
        {
            EventViewModel? model = null;
            using (var result = await _serviceApiClient.Client.GetAsync($"{id}"))
            {
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsAsync<EventViewModel>();
                    model = readTask;
                }
                else
                {
                    return NotFound();
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Unsubscribe(int id)
        {
            string? userId;
            string? userRole;

            if (IsUser(out userId, out userRole))
            {
                UserSubscription userSub = new UserSubscription { UserId = userId! };
                using (var result = await _serviceApiClient.Client.PostAsJsonAsync("unsubscribe/" + $"{id}", userSub))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else return BadRequest();
                }
            }
            return Ok();
        }

        [NonAction]
        private bool IsUser(out string userId, out string userRole)
        {
            bool isAuth = Convert.ToBoolean(HttpContext.Request.Cookies[CookiesKeys.IsAuthenticated]);
            userRole = HttpContext.Request.Cookies[CookiesKeys.UserRole];
            userId = HttpContext.Request.Cookies[CookiesKeys.UserId];
            if (isAuth & userRole == "User") return true;
            return false;
        }
    }
}
