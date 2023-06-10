using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs
{
	public class SignUpDTO
	{
		public string Name { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
