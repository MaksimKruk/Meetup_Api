#nullable disable
using MeetupApi.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetupApi.DataAccess
{
    public class MeetupContext : DbContext
    {
        public MeetupContext(DbContextOptions<MeetupContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Event { get; set; }
        public DbSet<SubscribedUser> SubscribedUser { get; set; }
    }
}
