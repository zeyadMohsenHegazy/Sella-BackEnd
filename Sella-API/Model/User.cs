using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;


namespace Sella_API.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } 

        public string Password { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string? ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpire { get; set; }
    }
}
