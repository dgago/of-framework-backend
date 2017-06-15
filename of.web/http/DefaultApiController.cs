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
		protected readonly IManager<TItem, TKey> Manager;

		public DefaultApiController(IManager<TItem, TKey> manager)
		{
			Manager = manager;
		}

		[PaginationFilter]
		public virtual async Task<IHttpActionResult> Get([FromUri] int? pageIndex = null, [FromUri] int? pageSize = null)
		{
			IEnumerable<KeyValuePair<string, string>> qs = Request.GetQueryNameValuePairs();
			Results<TItem> results = await Manager.FindAsync(User, qs, pageIndex ?? 1, pageSize ?? MAX_RECORDS);

			return UseViewModel ? OkCount(GetViewModel(results)) : OkCount(results);
		}

		public virtual async Task<IHttpActionResult> GetOne([FromUri] TKey id)
		{
			TItem res = await Manager.FindOneAsync(User, id);
			if (res == null)
			{
				return NotFound();
			}

			return Ok(UseViewModel ? GetViewModel(res) : res);
		}

		public virtual async Task<IHttpActionResult> Post(TItem item)
		{
			TKey res = await Manager.CreateAsync(User, item);
			return Created(res.ToString(), item);
		}

		public virtual async Task<IHttpActionResult> Put([FromUri] TKey id, [FromBody] TItem item)
		{
			await Manager.ReplaceAsync(User, id, item);
			return NoContent();
		}

		public virtual async Task<IHttpActionResult> Delete([FromUri] TKey id)
		{
			await Manager.DeleteAsync(User, id);
			return NoContent();
		}

		protected virtual Results<object> GetViewModel(Results<TItem> results)
		{
			return results.AsObjects<object>();
		}

		protected virtual object GetViewModel(TItem item)
		{
			return item;
		}
	}
}