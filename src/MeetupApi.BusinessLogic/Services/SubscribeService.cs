using MeetupApi.BusinessLogic.Interfaces;
using MeetupApi.DataAccess.Repository;
using MeetupApi.Infrastructure;

namespace MeetupApi.BusinessLogic.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly ISubscribeRepository _subscribeRepository;

        public SubscribeService(ISubscribeRepository subscribeRepository, IEventRepository eventRepository)
        {
            _subscribeRepository = subscribeRepository;
        }

        public async Task SubscribeAsync(int eventId, string userId)
        {
            await _subscribeRepository.SubscribeAsync(eventId, userId);
        }

        public async Task UnsubscribeAsync(int eventId, string userId)
        {
            await _subscribeRepository.UnsubscribeAsync(eventId, userId);
        }
    }
}
