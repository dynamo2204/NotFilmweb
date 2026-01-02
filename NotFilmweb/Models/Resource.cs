using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotFilmweb.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        // Klucz obcy do Kategorii
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Review> Reviews { get; set; }

        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                {
                    return 0.0;
                }
                return Math.Round(Reviews.Average(r => r.Rating), 1); // Średnia zaokrąglona do 1 miejsca po przecinku
            }
        }
    }
}
