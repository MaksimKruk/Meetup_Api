namespace MeetupApi.MvcApp.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Theme { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventTime { get; set; }
        public string Place { get; set; } = string.Empty;
        public bool IsSubscribed { get; set; } = false;
    }
}
