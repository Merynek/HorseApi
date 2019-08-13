using System.ComponentModel.DataAnnotations;

namespace HorsiApi.Models.BindingModels
{
    public class LoginBindingModel
    {
        [Required]
        [Display(Name = "email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "password")]
        public string password { get; set; }

        [Required]
        [Display(Name = "permanent")]
        public bool permanent { get; set; }
    }

    public class RefreshTokenBindingModel
    {
        [Required]
        [Display(Name = "token")]
        public string token { get; set; }

        [Required]
        [Display(Name = "refreshToken")]
        public string refreshToken { get; set; }
    }
}
