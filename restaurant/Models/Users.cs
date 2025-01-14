using Microsoft.AspNetCore.Identity;
namespace restaurant.Models
{
    public class Users : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Role {  get; set; }

        public string? Adresse { get; set; }

        public string? City { get; set; }



        
    }
}
