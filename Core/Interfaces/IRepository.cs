using System.Linq.Expressions;

namespace Core.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		Task SaveChangesAsync();

		Task<IEnumerable<TEntity>> GetAllAsync();

		Task<IEnumerable<TEntity>> GetAsync(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "");

		Task<TEntity> FindAsync(object id);

		Task AddAsync(TEntity entity);

		Task Remove(object id);

		void Remove(TEntity entityToDelete);

		void Update(TEntity entityToUpdate);
	}
}
