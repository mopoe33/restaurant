using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant.Models
{
    public class BookingTable
    {

        public int Id { get; set; }
        public string ClientId  { get; set; }

        public int TableId { get; set; }

        public DateTime bookingDateStart { get; set; }

        public DateTime BookingDateEnd { get; set; }

        public int serveurId { get; set; }

        [NotMapped]
        public int PersonNumber { get; set; }

        public string Status { get; set; }
    }
}
