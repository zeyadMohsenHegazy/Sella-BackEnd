using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace Sella_API.Model
{
    public class CartProducts
    {
        [Key, Column(Order = 0)]
        public int CartID { get; set; }

        [Key, Column(Order = 1)]
        public int ProductID { get; set; }


        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("CartID")]
        public virtual Cart Cart { get; set; }


    }
}
