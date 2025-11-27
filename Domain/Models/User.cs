using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class User
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; } = null!;
        [Required]
        public string Lastname { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Roles { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits and only contain numeric characters.")]
        [Required]
        public string Phone { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?s=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
    public class LoginUser
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        //[RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
