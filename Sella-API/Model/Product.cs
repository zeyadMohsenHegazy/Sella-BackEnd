using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
namespace Sella_API.Model
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "User Name must be less than 30 characters")]
        [MinLength(4, ErrorMessage = "User Name must be more than 4 characters")]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0,double.MaxValue)]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public string Color { get; set; }
        
        public string Description { get; set; }


        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }

       
        [ForeignKey("category")]
        public int CategoryID { get; set; }
        [JsonIgnore]
        public virtual Category category { get; set; }


       
        [InverseProperty("Product")]
        public virtual ICollection<OrderedProducts> OrderedProducts { get; set; }
        
        [InverseProperty("Product")]
        public virtual ICollection<CartProducts> CartProducts { get; set; }


    }
}
