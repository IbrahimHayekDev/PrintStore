using System.ComponentModel.DataAnnotations;

namespace PrintStoreApi.Models.Api;

public class ValidateAccountRequest
{
	[Required]
	public string Email { get; set; }
	[Required]
	public string Token { get; set; }
}
