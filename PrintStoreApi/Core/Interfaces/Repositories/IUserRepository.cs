using PrintStoreApi.Core.Entities.User;

namespace PrintStoreApi.Core.Interfaces.Repositories;

public interface IUserRepository
{
	Task<UserDB> getUserById(Guid id);
	Task editUser( UserDB userDTO);
}
