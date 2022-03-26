#nullable disable
namespace MeetupApi.MvcApp.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<EventViewModel> Events { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
