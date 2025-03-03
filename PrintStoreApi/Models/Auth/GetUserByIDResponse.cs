using PrintStoreApi.Models.User;

namespace PrintStoreApi.Models.Api;

public class GetUserByIDResponse
{
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MobileNumber { get; set; }
	public RoleDTO Role { get; set; }
}
