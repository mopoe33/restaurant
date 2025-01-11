using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace restaurant.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        [Range(1,9999.99)]
        public double prixUt { get; set; }
        public DateOnly  expirationDate { get; set; }
        public double Quantity { get; set; }

        
        public ICollection<PlatIngredient> PlatIngredients { get; set; } = new List<PlatIngredient>();

    }
}
