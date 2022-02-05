using System.ComponentModel.DataAnnotations;

namespace libry.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }

        [Required]
        [MaxLength(200)]
        public string Role { get; set; }
    }
}