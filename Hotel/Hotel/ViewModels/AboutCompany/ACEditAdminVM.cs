using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.AboutCompany
{
    public class ACEditAdminVM
    {

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(64)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(256)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Icon is required")]
        public string Icon { get; set; }

        [Required(ErrorMessage = "Latest videos URL is required")]
        public string LatestVideos { get; set; }

        [Required(ErrorMessage = "Service one name is required")]
        [MaxLength(32)]
        public string OurServiceOne { get; set; }

        [Required(ErrorMessage = "Service two name is required")]
        [MaxLength(32)]
        public string OurServiceTwo { get; set; }

        [Required(ErrorMessage = "Service description one is required")]
        [MaxLength(128)]
        public string ServiceDescriptionOne { get; set; }

        [Required(ErrorMessage = "Service description two is required")]
        [MaxLength(128)]
        public string ServiceDescriptionTwo { get; set; }

        [DataType(DataType.Upload)]
        [MaxLength(32)]
        public string CeoName { get; set; }


        [DataType(DataType.Upload)]
        public IFormFile? ImageFileOne { get; set; }
        public string? ImageUrlOne { get; set; }


        [DataType(DataType.Upload)]
        public IFormFile? ImageFileTwo { get; set; }
        public string? ImageUrlTwo { get; set; }


        [DataType(DataType.Upload)]
        public IFormFile? CeoImage { get; set; }
        public string? CeoImageUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? CeoSignature { get; set; }
        public string? CeoSignatureUrl { get; set; }
    }
}
