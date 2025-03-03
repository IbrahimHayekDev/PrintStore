using System.ComponentModel.DataAnnotations;

namespace PrintStoreApi.Models.Api;

public class LoginRequestDTO
{

	[Required, MaxLength(100), EmailAddress]
	public string Email { get; set; }

	[Required, MinLength(8)]
	public string Password { get; set; }
}
