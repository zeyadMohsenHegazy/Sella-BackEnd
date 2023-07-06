using System.ComponentModel.DataAnnotations.Schema;

namespace Sella_API.DTO
{
    public class ProductImagesDTO
    {
        public int ImageID { get; set; }
        public List<IFormFile> ImageURL { get; set; }
        public int ProductID { get; set; }
    }
}
