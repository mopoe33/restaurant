namespace restaurant.Models
{
    public class BookingTable
    {

        public int Id { get; set; }
        public string ClientId  { get; set; }

        public int TableId { get; set; }

        public DateTime bookingDate { get; set; }

        public int serveurId { get; set; }

        public string Status { get; set; }
    }
}
