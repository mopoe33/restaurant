namespace restaurant.Models
{
    public class Serveur 
    {

        public int Id { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }
        public TimeSpan timeStart { get; set; }

        public TimeSpan timeEnd { get; set; }
    }
}
