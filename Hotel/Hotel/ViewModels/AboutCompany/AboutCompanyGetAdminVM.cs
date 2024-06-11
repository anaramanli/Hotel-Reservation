using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.AboutCompany
{
    public class AboutCompanyGetAdminVM
    {
        public int Id { get; set; }
        public required string ImageUrlOne { get; set; }
        public required string ImageUrlTwo { get; set; }
        public required string Icon { get; set; }
        public required string LatestVideos { get; set; }

        [MaxLength(32)]
        public required string Title { get; set; }
        [MaxLength(128)]
        public required string Description { get; set; }
        [MaxLength(32)]
        public required string OurServiceOne { get; set; }
        [MaxLength(32)]
        public required string OurServiceTwo { get; set; }
        [MaxLength(128)]
        public required string ServiceDescriptionOne { get; set; }
        [MaxLength(128)]
        public required string ServiceDescriptionTwo { get; set; }


        public string? CeoImageUrl { get; set; }
        [MaxLength(32)]
        public required string CeoName { get; set; }
        public required string CeoSignatureUrl { get; set; }
    }
}
