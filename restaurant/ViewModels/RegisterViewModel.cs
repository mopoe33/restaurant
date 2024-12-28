using System.ComponentModel.DataAnnotations;

namespace restaurant.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nom est obligatoire.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Prenom est obligatoire.")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email est obligatoire.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telephone est obligatoire.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Ville est obligatoire.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Adresse est obligatoire.")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Mot de passe est obligatoire.")]
        [StringLength(40,MinimumLength = 8 , ErrorMessage ="le mot de passe doit contenir entre 8 et 40 caracter")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",ErrorMessage ="le mot de passe n'est pas identique")]
        public string Password { get; set; }
        [Required(ErrorMessage = "cofirmer le mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
