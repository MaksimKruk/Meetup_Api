#nullable disable
using MeetupApi.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetupApi.DataAccess.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly MeetupContext _context;

        public EventRepository(MeetupContext context)
        {
            _context = context;
        }

        public async Task DeleteItemAsync(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            //return await _context.Event.AsNoTracking().ToListAsync();
            return await _context.Event.ToListAsync();
        }

        public async Task<Event> GetItemAsync(int id)
        {
            return await _context.Event.FindAsync(id);
        }

        public async Task PostItemAsync(Event @event)
        {
            _context.Event.Add(@event);
            await _context.SaveChangesAsync();
        }

        public async Task PutItemAsync(Event @event)
        {
            _context.Entry(@event).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool ItemExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }
    }
}
