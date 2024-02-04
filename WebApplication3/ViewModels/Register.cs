using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApplication3.ViewModels
{
    public class Register
	{
		[Required(ErrorMessage = "First Name is required")]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name is required")]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Gender is required")]
		[DataType(DataType.Text)]
		public string Gender { get; set; }

		[Required(ErrorMessage = "NRIC is required")]
		[RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "NRIC must be alphanumeric")]
		public string NRIC { get; set; }

		[Required(ErrorMessage = "Email Address is required")]
		[DataType(DataType.EmailAddress)]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Birth date is required")]
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please select membership level")]
		[RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Special characters are not allowed")]
		public string WhoAmI { get; set; }

	}
}
