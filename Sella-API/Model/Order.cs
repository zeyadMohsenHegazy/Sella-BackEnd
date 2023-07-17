using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace Sella_API.Model
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "Order Date")]
        public DateTime Date { get; set; }


        [ForeignKey("customer")]
        public int CustomerID { get; set; }

        public virtual Customer customer { get; set; }
        

        [InverseProperty("Order")]
        public virtual ICollection<OrderedProducts> OrderedProducts { get; set; }

    }
}
