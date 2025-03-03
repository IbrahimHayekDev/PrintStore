using System.ComponentModel.DataAnnotations;

namespace PrintStoreApi.Models.Api;

public class RegisterRequestDTO
{
	[Required, MaxLength(50)]
	public string FirstName { get; set; }

	[Required, MaxLength(50)]
	public string LastName { get; set; }

	[MaxLength(50)]
	public string MobileNumber { get; set; }

	[Required, MaxLength(100), EmailAddress]
	public string Email { get; set; }

	[Required, MinLength(8)]
	public string Password { get; set; }
}
