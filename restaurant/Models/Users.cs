using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace restaurant.Models
{
    public class Users : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Role {  get; set; }

        public string? Adresse { get; set; }

        public string? City { get; set; }

        [NotMapped]
        public string? OldPassword { get; set; }

        [NotMapped]
        public string? NewPassword { get; set; }

        [NotMapped]
        public string? NewConfirmPassword { get; set; }


    }
}
