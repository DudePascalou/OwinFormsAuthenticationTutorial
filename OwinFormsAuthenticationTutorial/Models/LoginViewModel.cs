using System.ComponentModel.DataAnnotations;

namespace OwinFormsAuthenticationTutorial.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Identifiant")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
    }
}