using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Slider
{
    public class GetSliderVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
