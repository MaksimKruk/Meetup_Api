#nullable disable
namespace MeetupApi.Infrastructure
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public string Place { get; set; }
        public bool IsSubscribed { get; set; } = false;
    }
}