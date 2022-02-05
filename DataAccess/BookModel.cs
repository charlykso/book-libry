using libry.Models;

namespace libry.DataAccess
{
    public class BookModel
    {
        public int Id { get; set; }
        public string PublicationYear { get; set; } 
        public string Title { get; set; }  
        public Author Author { get; set; }
    }
}