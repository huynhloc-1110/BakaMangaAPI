using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs
{
	public class ExtendDTO
	{
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
	}
}
