using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace Sella_API.Model
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public double SubTotal { get; set; }

        [ForeignKey("user")]
        public int UserID { get; set; }

        public virtual User user { get; set; }


        [InverseProperty("Cart")]
        public virtual ICollection<CartProducts> CartProducts { get; set; }
    }
}
