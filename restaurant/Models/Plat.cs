namespace restaurant.Models
{
    public class Plat
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }
        public string? ImagePath { get; set; }

        public List<int> IngredientIds { get; set; }

        public ICollection<PlatIngredient>? PlatIngredients { get; set; }

        public Plat()
        {
            IngredientIds = new List<int>(); // Initialize the list
        }


    }
}
