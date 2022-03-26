using MeetupApi.Infrastructure;

namespace MeetupApi.BusinessLogic.Interfaces
{
    public interface IEventService : IService<EventDto>
    {
        Task<IEnumerable<EventDto>> GetAllAsync(string userId);
        Task<IEnumerable<EventDto>> GetSpecialEventsAsync(string userId);
    }
}
