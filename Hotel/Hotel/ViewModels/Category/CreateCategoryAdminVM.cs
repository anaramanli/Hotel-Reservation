using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Category
{
	public class CreateCategoryAdminVM
	{
		[Required]
		public string CategoryName { get; set; }
	}
}
