namespace MeetupApi.DataAccess.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Theme { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventTime { get; set; } 
        public string Place { get; set; } = string.Empty;
        public List<SubscribedUser> SubscribedUsers { get; set; } = new();
    }
}
