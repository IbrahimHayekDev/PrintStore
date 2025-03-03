using PrintStoreApi.Models.Api;
using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.User;

namespace PrintStoreApi.Core.Interfaces.Services;

public interface IUserService
{
	Task<Response<GetUserByIDResponse>> GetUserById(string userId);
	Task<Response<EditUserResponse>> EditUserAsync(string userId,EditUserRequest request);
}
