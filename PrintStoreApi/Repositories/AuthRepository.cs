using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.Auth;
using PrintStoreApi.Core.Entities.User;
using PrintStoreApi.Core.Interfaces.Repositories;
using PrintStoreApi.Data;

namespace PrintStoreApi.Repositories;

public class AuthRepository : IAuthRepository
{
	private readonly AppDbContext _context;

	public AuthRepository(AppDbContext context)
	{
		_context = context;
	}

	public async Task<UserDB> GetByEmailAsync(string email)
	{
		return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
	}

	public async Task AddAsync(UserDB user)
	{
		_context.Users.Add(user);
		await _context.SaveChangesAsync();
	}

	public async Task<bool> IsTokenRevokedAsync(string token)
	{
		return await _context.RevokedTokens.AnyAsync( t => t.Token == token);
	}

	public async Task RevokeTokenAsync(string token, DateTime expiryDate)
	{
		await _context.RevokedTokens.AddAsync( new RevokedToken
		{
			Token = token,
			ExpiryDate = expiryDate
		});
		await _context.SaveChangesAsync();
	}

}