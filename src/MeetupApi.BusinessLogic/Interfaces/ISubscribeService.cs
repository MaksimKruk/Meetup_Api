using MeetupApi.Infrastructure;

namespace MeetupApi.BusinessLogic.Interfaces
{
    public interface ISubscribeService
    {
        Task SubscribeAsync(int eventId, string userId);
        Task UnsubscribeAsync(int eventId, string userId);
    }
}
