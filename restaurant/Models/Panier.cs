namespace restaurant.Models
{
    public class Panier
    {
        public int Id { get; set; }
        public string ClientId { get; set; }

        public int PlatId { get; set; }

        public int Quantite  { get; set; }

        public double Total { get; set; }

    }
}
