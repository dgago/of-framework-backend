using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

using of.data;
using of.support.configuration.data;

namespace of.support.configuration
{
	public class SettingManager : ISettingManager
	{
		private readonly ISettingStore _store;

		public SettingManager(ISettingStore store)
		{
			_store = store;
		}

		public Task<Results<Setting>> FindAllAsync(IPrincipal user, int pageIndex, int pageSize, string sortBy)
		{
			return _store.FindAllAsync(pageIndex, pageSize, sortBy);
		}

		public Task<Results<Setting>> FindAsync(IPrincipal user, IEnumerable<KeyValuePair<string, string>> query, int pageIndex, int pageSize, string sortBy)
		{
			return _store.FindAsync(/*query.ToExpression<Setting>()*/ x => true, pageIndex, pageSize, sortBy);
		}

		public Task<Setting> FindOneAsync(IPrincipal user, string id)
		{
			return _store.FindOneAsync(id);
		}

		public Task<string> CreateAsync(IPrincipal user, Setting item)
		{
			throw new System.NotImplementedException();
		}

		public Task ReplaceAsync(IPrincipal user, string id, Setting item)
		{
			throw new System.NotImplementedException();
		}

		public Task DeleteAsync(IPrincipal user, string id)
		{
			throw new System.NotImplementedException();
		}
	}
}