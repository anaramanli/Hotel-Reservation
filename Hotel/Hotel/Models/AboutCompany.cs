using Hotel.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class AboutCompany : BaseEntity
    {
        public string ImageUrlOne { get; set; }
        public string ImageUrlTwo { get; set; }
        public string Icon { get; set; }
        public string LatestVideos { get; set; }

		public string Title { get; set; } = "Title";
		public string Description { get; set; }
        public string OurServiceOne { get; set; }
        public string OurServiceTwo { get; set; }
        public string ServiceDescriptionOne { get; set; }
        public string ServiceDescriptionTwo{ get; set; }


        public string CeoImageUrl { get; set; }
        public string CeoName { get; set; }
        public string CeoSignatureUrl { get; set; }
    }
}
