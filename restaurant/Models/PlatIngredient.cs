namespace restaurant.Models
{
    public class PlatIngredient
    {
        public int PlatId { get; set; }

        public Plat Plat { get; set; }

        public int IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }
        public double Quantity { get; set; }

    }
}
