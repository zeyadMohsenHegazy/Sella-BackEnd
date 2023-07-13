using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sella_DashBoard.Models
{
    public class Images
    {
        public int ImageID { get; set; }

        [Display(Name = "Image URL")]
        public string ImageURL { get; set; }

        public int ProductID { get; set; }
        public  Product product { get; set; }
    }
}
