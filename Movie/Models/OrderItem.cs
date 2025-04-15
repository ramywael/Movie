namespace Movie.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int MovieFilmId { get; set; }
        public MovieFilm MovieFilm { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }


    }
}
