using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

using of.data;

namespace of.support.diagnostics
{
	public class LogManager : ILogManager
	{
		public Task<Results<Log>> FindAllAsync(IPrincipal user, int pageIndex, int pageSize)
		{
			throw new System.NotImplementedException();
		}

		public Task<Results<Log>> FindAsync(IPrincipal user, IEnumerable<KeyValuePair<string, string>> query, int pageIndex, int pageSize)
		{
			throw new System.NotImplementedException();
		}

		public Task<Log> FindOneAsync(IPrincipal user, string id)
		{
			throw new System.NotImplementedException();
		}

		public Task<string> CreateAsync(IPrincipal user, Log item)
		{
			Support.LogProvider.Write(item.Type, item.Message, item.Values);
			return Task.FromResult(string.Empty);
		}

		public Task ReplaceAsync(IPrincipal user, string id, Log item)
		{
			throw new System.NotImplementedException();
		}

		public Task DeleteAsync(IPrincipal user, string id)
		{
			throw new System.NotImplementedException();
		}
	}
}