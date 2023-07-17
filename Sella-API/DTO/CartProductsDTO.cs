using Sella_API.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sella_API.DTO
{
    public class CartProductsDTO
    {

        public int CartID { get; set; }

        public int ProductID { get; set; }

    }
}
