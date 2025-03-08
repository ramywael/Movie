using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]  // ✅ This allows unlimited text storage

        public string Description { get; set; }
        [ValidateNever]
        public string CinemaLogo { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Address { get; set; }

        [ValidateNever]
        public List<MovieFilm> Movies { get; set; }
    }
}
