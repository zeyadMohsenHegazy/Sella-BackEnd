using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sella_DashBoard.Models
{
    public class ProductImagesDTO
    {
    

        public int ImageID { get; set; }

        public List<IFormFile> ImageURL { get; set; }


        public int ProductID { get; set; }
        
    }
}
