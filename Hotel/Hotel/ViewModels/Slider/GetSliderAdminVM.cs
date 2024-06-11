using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Slider
{
    public class GetSliderAdminVM
    {
        public int Id { get; set; }
        [MaxLength(100), Required]
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
