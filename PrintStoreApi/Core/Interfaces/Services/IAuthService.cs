using PrintStoreApi.Models.Api;
using PrintStoreApi.Models.Common;

namespace PrintStoreApi.Core.Interfaces.Services;

public interface IAuthService
{
	Task<Response<SignupResponseDTO>> RegisterAsync(RegisterRequestDTO request);
	Task<Response<LoginResponseDTO>> LoginAsync(LoginRequestDTO request);
}
