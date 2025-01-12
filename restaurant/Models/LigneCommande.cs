namespace restaurant.Models
{
    public class LigneCommande
    {
        public int Id { get; set; }
        public int CommandeId { get; set; }
        public int PlatId { get; set; }
        public int Quantite { get; set; }
        public double Total { get; set; }
    }
}
