using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
namespace Sella_API.Model
{
    public class ProductImages
    {
        [Key]
        public int ImageID { get; set; }
        public string ImageURL { get; set; }

        //forign key
        [ForeignKey("product")]
        public int ProductID { get; set; } 
        public virtual Product product { get; set; }
    }
}
