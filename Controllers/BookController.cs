using System.Linq;
using libry.DataAccess;
using libry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace libry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BooksContext _booksContext;

        public BookController(BooksContext booksContext)
        {
            _booksContext = booksContext;
        }

        //api/book/getall
        [HttpGet("getall")]
        public IActionResult GetAllBooks()
        {
            
            try
            {
                var books = _booksContext.Books.Include(a => a.Author).ToList();

                 if (books != null)
                {
                    if (books.Count > 0)
                    {
                        return Ok(books);
                    }
                    return BadRequest("Empty List");
                }
                return BadRequest("No Book found");
            }
            catch (System.Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
            
        }

        //api/book/get/{id}
        [Route("Get/{id}")]
        [HttpGet]
        public IActionResult Get(int Id)
        {
            var book = _booksContext.Books.Find(Id);

            if (book is null)
            {
                return BadRequest("Book not found");
            }
            return Ok(book);
        }

        [Route("create")]
        [HttpPost]
        public IActionResult post(BookModel book)
        {
            try
            {
                Book newBook = new Book();
                newBook.Title = book.Title;
                // newBook.Id = book.Id;
                newBook.PublicationYear = book.PublicationYear;
                newBook.Author = book.Author;

                // if (newBook.Id <= 0)
                // {
                //     return BadRequest("Invalid Id");
                // }

                if (string.IsNullOrEmpty(newBook.Title) ||
                    string.IsNullOrEmpty(newBook.PublicationYear) 
                )
                {
                    return BadRequest("Null value is not accepted!");
                }

                try
                {
                    _booksContext.Books.Add(newBook);
                    _booksContext.SaveChanges();

                    return Ok(newBook);
                }
                catch (System.Exception innerEx)
                {

                    return BadRequest(innerEx.Message);
                }
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}