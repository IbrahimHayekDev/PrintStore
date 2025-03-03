using PrintStoreApi.Core.Interfaces.Repositories;
using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.Api;
using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.User;

namespace PrintStoreApi.Services.User;

public class UserService: IUserService
{
	private readonly IUserRepository _userRepository;
	public UserService(IUserRepository userRepository)
	{
		_userRepository = userRepository;

	}
	public async Task<Response<GetUserByIDResponse>> GetUserById(string userId)
	{
		var response = new Response<GetUserByIDResponse>();
		if (String.IsNullOrEmpty(userId))
		{
			response.Error.Errors.Add("Token issue: User not found");
			return response;
		}
		var userIdGuid = new Guid(userId);
		var userData = await _userRepository.getUserById(userIdGuid);
		if (userData == null)
		{
			response.Error.Errors.Add("User not found");
			return response;
		}

		response.Data = new GetUserByIDResponse
		{
			Email = userData.Email,
			FirstName = userData.FirstName,
			LastName = userData.LastName,
			MobileNumber = userData.MobileNumber,
			Role = new RoleDTO
			{
				Id = userData.Role.Id,
				Name = userData.Role.Name,
			},
		};
		response.apiMessage = new ApiMessage
		{
			showMessage = false,
			message = "User Data Returned."
		};
		return response;
	}

	public async Task<Response<EditUserResponse>> EditUserAsync(string userId,EditUserRequest request)
	{
		var response = new Response<EditUserResponse>();

		var user = await _userRepository.getUserById(Guid.Parse(userId));
		if(user == null) {
			response.Error.Errors.Add("User not found");
			return response;
		}

			user.FirstName = request.FirstName;
			user.LastName = request.LastName;
			user.MobileNumber = request.MobileNumber;

		try
		{
			await _userRepository.editUser(user);
			var userData = await _userRepository.getUserById(Guid.Parse(userId));
			if (userData == null)
			{
				response.Error.Errors.Add("User not found");
				return response;
			}

			response.Data = new EditUserResponse
			{
				Email = userData.Email,
				FirstName = userData.FirstName,
				LastName = userData.LastName,
				MobileNumber = userData.MobileNumber,
				Role = new RoleDTO
				{
					Id = userData.Role.Id,
					Name = userData.Role.Name,
				},
			};
			response.apiMessage = new ApiMessage
			{
				showMessage = true,
				message = "User Data Updated."
			};
			return response;
		}
		catch (Exception ex) {
			response.Error.Errors.Add($"An error occured while updated user: {ex.Message}");
			return response;
		}
	}
}
