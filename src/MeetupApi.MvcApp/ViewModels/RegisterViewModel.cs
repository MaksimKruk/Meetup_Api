#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MeetupApi.MvcApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
