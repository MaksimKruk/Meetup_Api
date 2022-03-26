using MeetupApi.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetupApi.DataAccess.Repository
{
    public class SubscribeRepository : ISubscribeRepository
    {
        private readonly MeetupContext _context;

        public SubscribeRepository(MeetupContext context)
        {
            _context = context;
        }

        public async Task SubscribeAsync(int eventId, string userId)
        {
            var @event = await _context.Event.FirstOrDefaultAsync(param => param.Id == eventId);

            if (await _context.SubscribedUser.AnyAsync(param => param.Id == userId))
            {
                var user = await _context.SubscribedUser.FirstOrDefaultAsync(param => param.Id == userId);
                @event!.SubscribedUsers.Add(user!);
            }
            else
            {
                var user = new SubscribedUser { Id = userId };
                @event!.SubscribedUsers.Add(user!);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribeAsync(int eventId, string userId)
        {
            SubscribedUser? user = await _context.SubscribedUser.Include(s => s.Events).FirstOrDefaultAsync(s => s.Id == userId);
            Event? @event = _context.Event.FirstOrDefault(c => c.Id == eventId);
            if (@event != null && user != null)
            {
                @event!.SubscribedUsers.Remove(user);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetAllSubscribedEventsAsync()
        {
            return await _context.Event.Include(c => c.SubscribedUsers).ToListAsync();
        }
    }
}
