using System.ComponentModel.DataAnnotations;
using Humanizer.Localisation;

namespace NotFilmweb.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public string Name { get; set; }
        public ICollection<Resource>? Resources { get; set; }
    }
}
