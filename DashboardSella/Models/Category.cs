using System.ComponentModel.DataAnnotations;

namespace Sella_DashBoard.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        public string Discreption { get; set; }
    }
}
