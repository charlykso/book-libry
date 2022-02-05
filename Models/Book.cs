using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libry.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string PublicationYear { get; set; }
        
        [Required]
        public Author Author { get; set; }


        public override string ToString()
        {
            return $"{Title} ({PublicationYear})";
        }
    }
}