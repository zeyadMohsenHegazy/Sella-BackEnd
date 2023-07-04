using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sella_API.DTO
{
    public class ProductWithCategoryDTO
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

     
        public int Quantity { get; set; }


        public string Color { get; set; }

        public string Description { get; set; }


        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }

        public int CategoryID { get; set; }
    }
}
