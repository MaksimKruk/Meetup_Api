using MeetupApi.BusinessLogic.Interfaces;
using MeetupApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetupApi.ServiceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private IEventService _eventService;
        private ISubscribeService _subscribeService;

        public EventsController(IEventService eventService, ISubscribeService subscribeService)
        {
            _eventService = eventService;
            _subscribeService = subscribeService;
        }

        // GET: api/Events
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }

        // GET: api/Events
        [AllowAnonymous]
        [HttpGet("all/{userId}")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllAsync(string userId)
        {
            var events = await _eventService.GetAllAsync(userId);
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }

        // GET: api/Events
        [Authorize(Roles = "User")]
        [HttpGet("all/special/{userId}")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetSpecialAsync(string userId)
        {
            var events = await _eventService.GetSpecialEventsAsync(userId);
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }

        // GET: api/Events/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var @event = await _eventService.GetItemAsync(id);

            if (@event == null)
            {
                return NotFound();
            }
            return @event;
        }

        // PUT: api/Events/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventDto @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }
            try
            {
                await _eventService.PutItemAsync(id, @event);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Events
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<EventDto>> PostEvent(EventDto @event)
        {
            try 
            {
                await _eventService.PostItemAsync(@event);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _eventService.GetItemAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            try
            {
                await _eventService.DeleteItemAsync(@event.Id);
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return Ok();
        }

        private bool EventExists(int id)
        {
            return _eventService.EntityExists(id);
        }

        [Authorize(Roles = "User")]
        [HttpPost("subscribe/{id}")]
        public async Task<ActionResult<EventDto>> Subscribe(int id, UserSubscription userSub)
        {
            var @event = await _eventService.GetItemAsync(id);

            if (@event == null)
            {
                return NotFound();
            }
            try
            {
                await _subscribeService.SubscribeAsync(id, userSub.UserId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpPost("unsubscribe/{id}")]
        public async Task<ActionResult<EventDto>> Unsubscribe(int id, UserSubscription userSub)
        {
            var @event = await _eventService.GetItemAsync(id);

            if (@event == null)
            {
                return NotFound();
            }
            try
            {
                await _subscribeService.UnsubscribeAsync(id, userSub.UserId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
