
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Models
{
    public class MovieFilm
    {
        public int Id { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]  // ✅ This allows unlimited text storage
        public string Description { get; set; }

        [Required]
        [Range(0,100)]
        public double Price { get; set; }
        [ValidateNever]
        public string ImgUrl { get; set; }
        [Required]
        public string TrailerUrl { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [ValidateNever]
        public MovieStatus MovieStatus { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int CinemaId { get; set; }
        [ValidateNever]
        public Cinema cinema { get; set; }
        [ValidateNever]

        public Category category { get; set; }
        [ValidateNever]


        public List<Actor> actors { get; set; }
        [ValidateNever]


        public List<OrderItem> orderItems { get; set; }
        [ValidateNever]


        public List<ActorMovie> ActorMovies { get; set; }

    }
}
