using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Category
{
    public class EditCategoryAdminVM
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
