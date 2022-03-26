using MeetupApi.Infrastructure;
using MeetupApi.MvcApp.Utility.HttpClients;
using MeetupApi.MvcApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MeetupApi.MvcApp.Controllers
{
    public class EventController : Controller
    {
        private readonly ServiceApiClient _serviceApiClient;
        
        public EventController(ServiceApiClient serviceApiClient)
        {
            _serviceApiClient = serviceApiClient;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<EventViewModel>? model = null;
            using (var result = await _serviceApiClient.Client.GetAsync(String.Empty))
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
            }
            return View("Index", model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            return await GetById(id);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var result = await _serviceApiClient.Client.DeleteAsync($"{id}"))
            {
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else return NotFound();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventDto eventDto)
        {
            if (ModelState.IsValid)
            {
                using (var result = await _serviceApiClient.Client.PostAsJsonAsync(String.Empty, eventDto))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return Ok();
        }

        // GET: HomeController1/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await GetById(id);
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, EventDto eventDto)
        {
            if (id != eventDto.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                using (var result = await _serviceApiClient.Client.PutAsJsonAsync($"{id}", eventDto))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(eventDto);
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
    }
}
