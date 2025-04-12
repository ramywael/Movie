namespace Movie.Models
{
    public class Cart
    {
        public ApplicationUser User { get; set; }
        public string ApplicationUserId { get; set; }
        public MovieFilm Movie { get; set; }
        public int MovieFilmId { get; set; }
        public int Count { get; set; }
    }
}
