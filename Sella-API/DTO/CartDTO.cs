using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sella_API.DTO
{
    public class CartDTO
    {
        public int CartID { get; set; }
        public int Quantity { get; set; }
     
        public double SubTotal { get; set; }

        public int CustomerID { get; set; }
    }
}
