using Microsoft.EntityFrameworkCore;

namespace libry.Models
{
    public class BooksContext : DbContext
    {
        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {
            
        }
        public DbSet<Author> Authors { get; set; }  
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}