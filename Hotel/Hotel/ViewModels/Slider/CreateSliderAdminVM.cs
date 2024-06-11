using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Slider
{
    public class CreateSliderAdminVM
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
    }
}
