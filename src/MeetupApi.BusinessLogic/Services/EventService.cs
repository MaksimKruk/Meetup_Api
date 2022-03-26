#nullable disable
using AutoMapper;
using MeetupApi.BusinessLogic.Interfaces;
using MeetupApi.DataAccess.Models;
using MeetupApi.DataAccess.Repository;
using MeetupApi.Infrastructure;

namespace MeetupApi.BusinessLogic.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ISubscribeRepository _subscribeRepository;

        public EventService(IEventRepository eventRepository, ISubscribeRepository subscribeRepository)
        {
            _eventRepository = eventRepository;
            _subscribeRepository = subscribeRepository;
        }

        public async Task DeleteItemAsync(int id)
        {
            await _eventRepository.DeleteItemAsync(id);
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Event, EventDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Event>, List<EventDto>>(await _eventRepository.GetAllAsync());
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync(string userId)
        {
            var models = await GetAllAsync();
            if(userId == "null") return models;

            var subModels = await _subscribeRepository.GetAllSubscribedEventsAsync();

            var matchModels = subModels.SelectMany(u => u.SubscribedUsers,
                            (u, l) => new { Event = u, User = l })
                          .Where(u => u.User.Id == userId)
                          .Select(u => u.Event);

            foreach (var dto in models)
            {
                if(matchModels.Any(param => param.Id == dto.Id))
                {
                    dto.IsSubscribed = true;
                }
                else
                {
                    dto.IsSubscribed = false;
                }
            }
            return models;
        }

        public async Task<IEnumerable<EventDto>> GetSpecialEventsAsync(string userId)
        {
            var models = await GetAllAsync();
            if (userId == "null") return null;

            var subModels = await _subscribeRepository.GetAllSubscribedEventsAsync();

            var matchModels = subModels.SelectMany(u => u.SubscribedUsers,
                            (u, l) => new { Event = u, User = l })
                          .Where(u => u.User.Id == userId)
                          .Select(u => u.Event);

            foreach (var dto in models)
            {
                if (matchModels.Any(param => param.Id == dto.Id))
                {
                    dto.IsSubscribed = true;
                }
                else
                {
                    dto.IsSubscribed = false;
                }
            }
            return models.Where(param => param.IsSubscribed == true);
        }

        public async Task<EventDto> GetItemAsync(int id)
        {
            var @event = await _eventRepository.GetItemAsync(id);
            if (@event == null)
            {
                return null;
            }
            return new EventDto
            {
                Id = @event.Id,
                Theme = @event.Theme,
                Description = @event.Description,
                EventTime = @event.EventTime,
                Place = @event.Place
            };
        }

        public async Task PostItemAsync(EventDto @event)
        {
            Event newEventSample = new Event
            {
                Theme = @event.Theme,
                Description = @event.Description,
                EventTime = @event.EventTime,
                Place = @event.Place
            };
            await _eventRepository.PostItemAsync(newEventSample);
        }

        public async Task PutItemAsync(int id, EventDto @event)
        {
            var putEventSample = _eventRepository.GetItemAsync(id);
            SetFields(await putEventSample, @event);
            await _eventRepository.PutItemAsync(await putEventSample);
        }

        public bool EntityExists(int id)
        {
            return _eventRepository.ItemExists(id);
        }

        private void SetFields(Event putEventSample, EventDto @event)
        {
            putEventSample.Theme = @event.Theme;
            putEventSample.Description = @event.Description;
            putEventSample.EventTime = @event.EventTime;
            putEventSample.Place = @event.Place;
        }
    }
}
