namespace restaurant.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public String? ClientId { get; set; }
        public DateTime DateCommande { get; set; }
        public double Total { get; set; }
        public string Statut { get; set; }
        public List<LigneCommande>? LignesCommande { get; set; }
    }
}
