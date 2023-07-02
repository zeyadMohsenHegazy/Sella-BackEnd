using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;


namespace Sella_API.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        
        [Required]
        [MaxLength(10,ErrorMessage ="User Name must be less than 10 characters")]
        [MinLength(4,ErrorMessage = "User Name must be more than 4 characters")]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
