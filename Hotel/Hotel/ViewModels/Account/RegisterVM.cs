using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name is required"), MaxLength(64)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required"), MaxLength(64)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string RepeatPassword { get; set; }
    }
}
