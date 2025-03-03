using PrintStoreApi.Core.Entities.Base;

namespace PrintStoreApi.Core.Interfaces.Reprositories.Base;

public interface IReprository<T> where T : Entity
{
	Task<IReadOnlyList<T>> GetAllAsync();
	Task<T> GetByIdAsync(int id);
	Task<T> AddAsync(T entity);
	Task<T> UpdateAsync(T entity);
	Task DeleteAsync(T entity);
}
