namespace MeetupApi.DataAccess.Models
{
    public class SubscribedUser
    {
        public string Id { get; set; }
        public List<Event> Events { get; set; } = new();

    }
}
