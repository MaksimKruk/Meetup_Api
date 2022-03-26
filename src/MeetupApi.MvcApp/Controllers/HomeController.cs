using MeetupApi.Infrastructure;
using MeetupApi.MvcApp.Models;
using MeetupApi.MvcApp.Utility;
using MeetupApi.MvcApp.Utility.HttpClients;
using MeetupApi.MvcApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MeetupApi.MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ServiceApiClient _serviceApiClient;

        private readonly CheckTokenData _checkTokenData;

        public HomeController(ServiceApiClient serviceApiClient, CheckTokenData checkTokenData)
        {
            _serviceApiClient = serviceApiClient;
            _checkTokenData = checkTokenData;
        }

        public async Task<ActionResult> Index(string theme, int page = 1)
        {
            string? userId = HttpContext.Request.Cookies[CookiesKeys.UserId];
            if (string.IsNullOrEmpty(userId)) userId = "null";

            IEnumerable<EventViewModel>? model = null;
            using (var result = await _serviceApiClient.Client.GetAsync("all/" + userId))
            {
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsAsync<IList<EventViewModel>>();
                    model = readTask;
                }
                else
                {
                    return BadRequest();
                }
                if (!String.IsNullOrEmpty(theme))
                {
                    model = model.Where(p => p.Theme.Contains(theme));
                }
            }
            var viewModel = ConfugurePagination(model, page);
            return View("Index", viewModel);
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

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> Subscribe(int id)
        {
            string? userId;
            string? userRole;

            if (CheckTokenData._isInvalid(HttpContext.Request.Cookies[CookiesKeys.AccessToken]) & !string.IsNullOrEmpty(HttpContext.Request.Cookies[CookiesKeys.AccessToken]))
            {
                await _checkTokenData.RefreshAsync();
            }

            if (IsUser(out userId, out userRole))
            {
                UserSubscription userSub = new UserSubscription { UserId = userId! };
                using (var result = await _serviceApiClient.Client.PostAsJsonAsync("subscribe/" + $"{id}", userSub))
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

        public async Task<ActionResult> Unsubscribe(int id)
        {
            string? userId;
            string? userRole;

            if (IsUser(out userId,out userRole))
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
        private IndexViewModel ConfugurePagination(IEnumerable<EventViewModel>? model, int page)
        {
            var pageSize = 3;
            var count = model!.Count();
            var items = model!.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Events = items
            };
            return viewModel;
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