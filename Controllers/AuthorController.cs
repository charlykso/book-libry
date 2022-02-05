using System;
using System.Linq;
using libry.DataAccess;
using libry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace libry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly BooksContext _booksContext;
        public AuthorController(BooksContext booksContext)
        {
            _booksContext = booksContext;
        }

        [Authorize]
        //api/author/getall
        [HttpGet("getall")]
        public IActionResult GetAllAuthors()
        {
            try
            {
                var authors = _booksContext.Authors.ToList();

                if (authors != null)
                {

                    return Ok(authors);

                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex);

            }
            return BadRequest("NOT FOUND");


        }

        //api/authors/get/{id}
        [Route("Get/{id}")]
        [HttpGet]
        public IActionResult Get(int Id)
        {
            var author = _booksContext.Authors.Find(Id);

            if (author is null)
            {
                return BadRequest("Author not found");
            }
            return Ok(author);
        }

        [Route("create")]
        [HttpPost]
        public IActionResult post(AuthorModel author)
        {
            try
            {
                Author newAuthor = new Author();
                newAuthor.Name = author.Name;

                if (string.IsNullOrEmpty(author.Name))
                {
                    return BadRequest(" Please null value is not accepted");
                }

                try
                {
                    _booksContext.Authors.Add(newAuthor);
                    _booksContext.SaveChanges();

                    return Ok(newAuthor);
                }
                catch (System.Exception inEx)
                {

                    return BadRequest(inEx.Message);
                }
            }
            catch (System.Exception exc)
            {

                return BadRequest(exc.Message);
            }
        }

        [Route("Edit")]
        [HttpPut]
        public IActionResult put(AuthorModel author)
        {
            try
            {
                var editAuthor = _booksContext.Authors.Find(author.Id);

                if (editAuthor is null)
                {
                    return BadRequest("Author not found");
                }
                editAuthor.Name = author.Name;
                editAuthor.Id = author.Id;

                if (editAuthor.Id <= 0)
                {
                    return BadRequest("Invalid Id");
                }
                if (string.IsNullOrEmpty(editAuthor.Name))
                {
                    return BadRequest("Please null value is not accepted");
                }

                try
                {
                    _booksContext.Authors.Attach(editAuthor);
                    _booksContext.SaveChanges();

                    return Ok(editAuthor);
                }
                catch (System.Exception exc)
                {

                    return BadRequest(exc.Message);
                }
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var author = _booksContext.Authors.Find(id);

            if (author is null)
            {
                return BadRequest("Author not found");
            }

            try
            {
                _booksContext.Authors.Remove(author);
                _booksContext.SaveChanges();

                return Ok("Author Successfully deleted...");
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}