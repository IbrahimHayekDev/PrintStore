using PrintStoreApi.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using PrintStoreApi.Data;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;

namespace PrintStoreApi.Reprositories.Base;

public class Reprository<T>: IReprository<T> where T: Entity
{
	protected  AppDbContext _context { get; set; }
	public Reprository(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IReadOnlyList<T>> GetAllAsync()
	{
		return await _context.Set<T>().ToListAsync();
	}

	public async Task<T> GetByIdAsync(int id)
	{
		return await _context.Set<T>().FindAsync(id);
	}

	public async Task<T> AddAsync(T entity)
	{
		await _context.Set<T>().AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<T> UpdateAsync(T entity)
	{
		_context.Entry(entity).State = EntityState.Modified;
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(T entity)
	{
		_context.Set<T>().Remove(entity);
		await _context.SaveChangesAsync();
	}
}
