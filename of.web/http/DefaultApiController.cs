using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using of.data;
using of.web.http.filter;

namespace of.web.http
{
	public abstract class DefaultApiController<TItem, TKey> : BaseApiController
	{
		private readonly IManager<TItem, TKey> _manager;

		public DefaultApiController(IManager<TItem, TKey> manager)
		{
			_manager = manager;
		}

		[PaginationFilter]
		public async Task<IHttpActionResult> Get([FromUri] int? pageIndex = null, [FromUri] int? pageSize = null)
		{
			IEnumerable<KeyValuePair<string, string>> qs = Request.GetQueryNameValuePairs();
			object es = await FindAllAsync(qs, pageIndex, pageSize);
			return Ok(es);
		}

		public async Task<IHttpActionResult> GetOne([FromUri] TKey id)
		{
			object res = await FindOneAsync(id);
			if (res == null)
			{
				return NotFound();
			}
			return Ok(res);
		}

		public async Task<IHttpActionResult> Post(TItem item)
		{
			TKey res = await _manager.CreateAsync(User, item);
			return Created(res.ToString(), item);
		}

		public async Task<IHttpActionResult> Put([FromUri] TKey id, [FromBody] TItem item)
		{
			await _manager.ReplaceAsync(User, id, item);
			return NoContent();
		}

		public async Task<IHttpActionResult> Delete([FromUri] TKey id)
		{
			await _manager.DeleteAsync(User, id);
			return NoContent();
		}

		#region helpers

		protected virtual async Task<object> FindAllAsync(IEnumerable<KeyValuePair<string, string>> qs, int? pageIndex, int? pageSize)
		{
			return await _manager.FindAsync(User, qs, pageIndex ?? 1, pageSize ?? MAX_RECORDS);
		}

		protected virtual async Task<object> FindOneAsync(TKey id)
		{
			return await _manager.FindOneAsync(User, id);
		}

		#endregion
	}
}