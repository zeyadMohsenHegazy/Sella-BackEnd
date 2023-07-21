using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace Sella_API.Model
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "OrderDate")]
        public DateTime OrderDate { get; set; }

        [ForeignKey("user")]
        public int UserID { get; set; }

        public virtual User user { get; set; }
        
        [InverseProperty("Order")]
        public virtual ICollection<OrderedProducts> OrderedProducts { get; set; }
    }
}
