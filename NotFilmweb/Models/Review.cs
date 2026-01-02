using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NotFilmweb.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ocena jest wymagana.")]
        [Range(1, 5, ErrorMessage = "Ocena musi być w przedziale 1-5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Treść recenzji jest wymagana.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Recenzja musi mieć od 10 do 500 znaków.")]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ResourceId { get; set; }
        public Resource Resource { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
