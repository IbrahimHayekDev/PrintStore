using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.User;
using PrintStoreApi.Core.Interfaces.Repositories;
using PrintStoreApi.Data;

namespace PrintStoreApi.Repositories;

public class UserRepository: IUserRepository
{
	private readonly AppDbContext _context;

	public UserRepository(AppDbContext context)
	{
		_context = context;
	}
	public async Task<UserDB> getUserById(Guid id)
	{
		return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
	}

	public async Task editUser(UserDB user)
	{
		_context.Users.Update(user);
		await _context.SaveChangesAsync();
	}
}
