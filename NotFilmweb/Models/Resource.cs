using System.ComponentModel.DataAnnotations;

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
    }
}
