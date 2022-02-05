using System;
using System.Security.Claims;
using System.Linq;
using libry.DataAccess;
using libry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace libry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly BooksContext _booksContext;

        public UserController(BooksContext booksContext)
        {
            _booksContext = booksContext;
        }

        //api/user
        [HttpGet]
        // [Authorize(Roles = "Admin")]
        public UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    Id = Int32.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    FirstName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    LastName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                    Password = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Hash)?.Value
                };
            }
            return null;
        }


        //api/user/getall
        [HttpGet("getall")]
        public IActionResult GetAllUsers()
        {
            var user = _booksContext.Users.ToList();

            if (user != null)
            {
                if (user.Count > 0)
                {
                    return Ok(user);
                }
                return BadRequest("Empty List");
            }

            return BadRequest("No User found");
        }


        [Route("Get/{id}")]
        [HttpGet]
        public IActionResult Get(int Id)
        {
            var user = _booksContext.Users.Find(Id);

            if (user is null)
            {
                return BadRequest("User not found");
            }
            return Ok(user);
        }


        [Route("create")]
        [HttpPost]
        public IActionResult post(UserModel user)
        {
            try
            {
                User NewUser = new User();
                NewUser.FirstName = user.FirstName;
                NewUser.LastName = user.LastName;
                NewUser.Email = user.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                NewUser.Password = user.Password;
                NewUser.Role = user.Role;

                if (string.IsNullOrEmpty(NewUser.FirstName) ||
                    string.IsNullOrEmpty(NewUser.LastName) ||
                    string.IsNullOrEmpty(NewUser.Email) ||
                    string.IsNullOrEmpty(NewUser.Password) ||
                    string.IsNullOrEmpty(NewUser.Role)
                )
                {
                    return BadRequest("Null value is not accepted!");
                }

                try
                {
                    _booksContext.Users.Add(NewUser);
                    _booksContext.SaveChanges();

                    return Ok(NewUser);
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

        [Route("Edit")]
        [HttpPut]
        public IActionResult put(UserModel user)
        {
            try
            {
                var editUser = _booksContext.Users.Find(user.Id);

                if (editUser is null)
                {
                    return BadRequest("User not found");
                }
                editUser.Id = user.Id;
                editUser.FirstName = user.FirstName;
                editUser.LastName = user.LastName;
                editUser.Email = user.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                editUser.Password = user.Password;
                editUser.Role = user.Role;

                if (editUser.Id <= 0)
                {
                    return BadRequest("Invalid Id");
                }

                if (string.IsNullOrEmpty(editUser.FirstName) ||
                    string.IsNullOrEmpty(editUser.LastName) ||
                    string.IsNullOrEmpty(editUser.Email) ||
                    string.IsNullOrEmpty(editUser.Password) ||
                    string.IsNullOrEmpty(editUser.Role)
                // editUser.Id == '';           
                )
                {
                    return BadRequest("Please null value is not accepted");
                }

                try
                {
                    _booksContext.Users.Attach(editUser);
                    _booksContext.SaveChanges();

                    return Ok(editUser);
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
            var user = _booksContext.Users.Find(id);

            if (user is null)
            {
                return BadRequest("User not found");
            }

            try
            {
                _booksContext.Users.Remove(user);
                _booksContext.SaveChanges();

                return Ok("User Successfully deleted...");
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}