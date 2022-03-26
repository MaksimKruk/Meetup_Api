using MeetupApi.DataAccess.Models;

namespace MeetupApi.DataAccess.Repository
{
    public interface ISubscribeRepository
    {
        Task SubscribeAsync(int eventId, string userId);
        Task UnsubscribeAsync(int eventId, string userId);
        Task<IEnumerable<Event>> GetAllSubscribedEventsAsync();
    }
}
