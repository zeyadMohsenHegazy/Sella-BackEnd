using System.ComponentModel.DataAnnotations.Schema;

namespace Sella_API.DTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
    
        public DateTime OrderDate { get; set; }

        public int UserID { get; set; }
    }
}
