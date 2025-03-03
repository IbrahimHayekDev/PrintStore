using PrintStoreApi.Core.Entities.User;

namespace PrintStoreApi.Core.Interfaces.Repositories;

public interface IAuthRepository
{
	Task<UserDB> GetByEmailAsync(string email);
	Task AddAsync(UserDB user);
	Task<bool> IsTokenRevokedAsync(string token);
	Task RevokeTokenAsync(string token, DateTime expiryDate);
}
