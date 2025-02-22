namespace Movie.Models
{
    public class ActorMovie
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int MovieFilmId { get; set; }
        public MovieFilm MovieFilm { get; set; }
    }
}
