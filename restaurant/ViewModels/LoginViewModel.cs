using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace restaurant.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email est obligatoire")]
        public string Email { get; set; }

        [Required(ErrorMessage = "mot de passe est obligatoire")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "remember me")]
        public bool RememberMe { get; set; }    




    }
}
