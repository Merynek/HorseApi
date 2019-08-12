using System.ComponentModel.DataAnnotations;

namespace HorseApi.Models.BindingModels
{
    public class RegistrationBindingModel
    {
        [Required]
        [Display(Name = "username")]
        public string username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "password")]
        public string password { get; set; }
    }
}
