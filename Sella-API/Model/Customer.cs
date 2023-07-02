using System.ComponentModel.DataAnnotations;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
namespace Sella_API.Model
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        
        [Required]
        [MaxLength(40, ErrorMessage = "User Name must be less than 10 characters")]
        [MinLength(12, ErrorMessage = "User Name must be more than 4 characters")] 
        public string CustomerName { get; set; }

        [Required]
        public string StreetName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Governate { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Please enter a valid 11-digit phone number.")]
        public string Phone { get; set; }
    }
}
