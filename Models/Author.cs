using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libry.Models
{
    public class Author
    {
        public int Id { get; set; } 

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        // public ICollection<Book> Books { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}