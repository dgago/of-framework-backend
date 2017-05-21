using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace of.data
{
	public class MongoDbStore<TEntity, TKey> : IStore<TEntity, TKey>
	{
		public MongoDbStore(IMongoCollection<TEntity> collection)
		{
			Collection = collection;
		}

		protected IMongoCollection<TEntity> Collection { get; }

		public TEntity FindOne(TKey id)
		{
			IFindFluent<TEntity, TEntity> res = Collection.Find(Builders<TEntity>.Filter.Eq("_id", id));
			return res.FirstOrDefault();
		}

		public TEntity FindOne(Expression<Func<TEntity, bool>> exp, object sortBy = null)
		{
			IFindFluent<TEntity, TEntity> res = FindFluent(exp, sortBy);
			return res.FirstOrDefault();
		}

		public List<TEntity> Find(Expression<Func<TEntity, bool>> exp, object sortBy = null)
		{
			IFindFluent<TEntity, TEntity> res = FindFluent(exp, sortBy);
			return res.ToList();
		}

		public Results<TEntity> Find(Expression<Func<TEntity, bool>> exp, int pageIndex, int pageSize, object sortBy = null)
		{
			IFindFluent<TEntity, TEntity> res = FindFluent(exp, pageIndex, pageSize, sortBy);
			return new Results<TEntity>(res.ToList(), res.Count(), pageIndex, pageSize);
		}

		public List<TEntity> FindAll(object sortBy = null)
		{
			BsonDocument doc = new BsonDocument();
			return FindFluent(doc, sortBy).ToList();
		}

		public Results<TEntity> FindAll(int pageIndex, int pageSize, object sortBy = null)
		{
			BsonDocument doc = new BsonDocument();
			IFindFluent<TEntity, TEntity> res = FindFluent(doc, pageIndex, pageSize, sortBy);
			return new Results<TEntity>(res.ToList(), res.Count(), pageIndex, pageSize);
		}

		public Task<TEntity> FindOneAsync(TKey id)
		{
			IFindFluent<TEntity, TEntity> res = Collection.Find(Builders<TEntity>.Filter.Eq("_id", id));
			return res.FirstOrDefaultAsync();
		}

		public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> exp, object sortBy = null)
		{
			IFindFluent<TEntity, TEntity> res = FindFluent(exp, sortBy);
			return res.FirstOrDefaultAsync();
		}

		public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> exp, object sortBy = null)
		{
			IFindFluent<TEntity, TEntity> res = FindFluent(exp, sortBy);
			return res.ToListAsync();
		}

		public async Task<Results<TEntity>> FindAsync(Expression<Func<TEntity, bool>> exp, int pageIndex, int pageSize, object sortBy = null)
		{
			IFindFluent<TEntity, TEntity> res = FindFluent(exp, pageIndex, pageSize, sortBy);

			return await FindPaginatedAsync(pageIndex, pageSize, res);
		}

		public Task<List<TEntity>> FindAllAsync(object sortBy = null)
		{
			BsonDocument doc = new BsonDocument();
			IFindFluent<TEntity, TEntity> res = FindFluent(doc, sortBy);
			return res.ToListAsync();
		}

		public async Task<Results<TEntity>> FindAllAsync(int pageIndex, int pageSize, object sortBy = null)
		{
			BsonDocument doc = new BsonDocument();
			IFindFluent<TEntity, TEntity> res = FindFluent(doc, pageSize, pageIndex, sortBy);

			return await FindPaginatedAsync(pageIndex, pageSize, res);
		}

		public bool Exists(Expression<Func<TEntity, bool>> exp)
		{
			return FindFluent(exp).Any();
		}

		public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> exp)
		{
			return FindFluent(exp).AnyAsync();
		}

		public TKey Create(TEntity item)
		{
			Collection.InsertOne(item);
			TKey v = (TKey)item?.GetType().GetProperty("Id")?.GetValue(item, null);
			return v;
		}

		public Task CreateAsync(TEntity item)
		{
			return Collection.InsertOneAsync(item);
		}

		public void Update(TKey id, TEntity item)
		{
			Collection.ReplaceOne(Builders<TEntity>.Filter.Eq("_id", id), item);
		}

		public Task UpdateAsync(TKey id, TEntity item)
		{
			FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", id);
			return Collection.ReplaceOneAsync(filter, item);
		}

		public void Remove(TKey id)
		{
			Collection.DeleteOne(Builders<TEntity>.Filter.Eq("_id", id));
		}

		public Task RemoveAsync(TKey id)
		{
			FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", id);
			return Collection.DeleteOneAsync(filter);
		}

		public void Remove(Expression<Func<TEntity, bool>> exp)
		{
			Collection.DeleteMany(exp);
		}

		public Task RemoveAsync(Expression<Func<TEntity, bool>> exp)
		{
			return Collection.DeleteManyAsync(exp);
		}

		#region helpers

		protected IFindFluent<TEntity, TEntity> FindFluent(Expression<Func<TEntity, bool>> exp, object sortBy = null)
		{
			FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(exp);
			return FindFluent(filter, sortBy);
		}

		protected IFindFluent<TEntity, TEntity> FindFluent(Expression<Func<TEntity, bool>> exp, int pageIndex, int pageSize, object sortBy = null)
		{
			FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(exp);
			IFindFluent<TEntity, TEntity> res = FindFluent(filter, sortBy);
			res.Skip((pageIndex - 1) * pageSize);
			res.Limit(pageSize);
			return res;
		}

		protected IFindFluent<TEntity, TEntity> FindFluent(BsonDocument doc, object sortBy = null)
		{
			FilterDefinition<TEntity> filter = new BsonDocumentFilterDefinition<TEntity>(doc);
			return FindFluent(filter, sortBy);
		}

		protected IFindFluent<TEntity, TEntity> FindFluent(BsonDocument doc, int pageIndex, int pageSize, object sortBy = null)
		{
			FilterDefinition<TEntity> filter = new BsonDocumentFilterDefinition<TEntity>(doc);
			IFindFluent<TEntity, TEntity> res = FindFluent(filter, sortBy);
			res.Skip((pageIndex - 1) * pageSize);
			res.Limit(pageSize);
			return res;
		}

		protected IFindFluent<TEntity, TEntity> FindFluent(FilterDefinition<TEntity> filter, object sortBy = null)
		{
			IFindFluent<TEntity, TEntity> res;
			if (sortBy != null)
			{
				BsonDocument document = sortBy as BsonDocument;
				SortDefinition<TEntity> sort = document != null
															? (SortDefinition<TEntity>)new BsonDocumentSortDefinition<TEntity>(document)
															: new ObjectSortDefinition<TEntity>(sortBy);
				res = Collection.Find(filter).Sort(sort);
			}
			else
			{
				res = Collection.Find(filter);
			}

			return res;
		}

		protected static async Task<Results<TEntity>> FindPaginatedAsync(int pageIndex, int pageSize, IFindFluent<TEntity, TEntity> res)
		{
			long count = await res.CountAsync();
			List<TEntity> items = await res.ToListAsync();

			return new Results<TEntity>(items, count, pageIndex, pageSize);
		}

		#endregion
	}
}