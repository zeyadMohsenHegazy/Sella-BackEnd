namespace Sella_DashBoard.Models
{
    public class Product
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

        public Category category { get; set; }
    }
}
