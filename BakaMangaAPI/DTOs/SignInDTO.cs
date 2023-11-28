using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs
{
	public class SignInDTO
	{
		[EmailAddress]
        public string Email { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;
	}
}
