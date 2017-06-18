using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace of.data
{
	public interface IStore<TEntity, TKey>
	{
		TEntity FindOne(TKey id);

		TEntity FindOne(Expression<Func<TEntity, bool>> exp, object sortBy = null);

		List<TEntity> Find(Expression<Func<TEntity, bool>> exp, object sortBy = null);

		Results<TEntity> Find(Expression<Func<TEntity, bool>> exp, int pageIndex, int pageSize, object sortBy = null);

		List<TEntity> FindAll(object sortBy = null);

		Results<TEntity> FindAll(int pageIndex, int pageSize, object sortBy = null);



		Task<TEntity> FindOneAsync(TKey id);

		Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> exp, object sortBy = null);

		Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> exp, object sortBy = null);

		Task<Results<TEntity>> FindAsync(Expression<Func<TEntity, bool>> exp, int pageIndex, int pageSize, object sortBy = null);

		Task<List<TEntity>> FindAllAsync(object sortBy = null);

		Task<Results<TEntity>> FindAllAsync(int pageIndex, int pageSize, object sortBy = null);



		bool Exists(Expression<Func<TEntity, bool>> exp);

		Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> exp);

		TKey Create(TEntity item);

		Task CreateAsync(TEntity item);

		void Update(TKey id, TEntity item);

		Task UpdateAsync(TKey id, TEntity item);

		void Remove(TKey id);

		Task RemoveAsync(TKey id);

		void Remove(Expression<Func<TEntity, bool>> exp);

		Task RemoveAsync(Expression<Func<TEntity, bool>> exp);



		object GetSortObject(string sortBy);
	}
}