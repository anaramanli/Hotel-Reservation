using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Slider
{
    public class UpdateSliderAdminVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string? ImageUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
    }
}
