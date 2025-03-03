using System.ComponentModel.DataAnnotations;

namespace PrintStoreApi.Core.Entities.Auth;

public class RevokedToken
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(500)]
	public string Token { get; set; }

	[Required]
	public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;

}
