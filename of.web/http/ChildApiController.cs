using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using of.data;
using of.web.http.filter;

namespace of.web.http
{
	public abstract class ChildApiController<TItem, TKey> : BaseApiController
	{
		protected readonly IChildManager<TItem, TKey> Manager;

		public ChildApiController(IChildManager<TItem, TKey> manager)
		{
			Manager = manager;
		}

		[PaginationFilter]
		public virtual async Task<IHttpActionResult> Get([FromUri] TKey id, [FromUri] int? pageIndex = null, [FromUri] int? pageSize = null, [FromUri] string sortBy = null)
		{
			IEnumerable<KeyValuePair<string, string>> qs = Request.GetQueryNameValuePairs();
			Results<TItem> results = await Manager.Find(User, id, qs, pageIndex ?? 1, pageSize ?? MAX_RECORDS, sortBy);

			return UseViewModel ? OkCount(GetViewModel(results)) : OkCount(results);
		}

		public virtual async Task<IHttpActionResult> GetOne([FromUri] TKey id, [FromUri] TKey childId)
		{
			TItem res = await Manager.FindOne(User, id, childId);
			if (res == null)
			{
				return NotFound();
			}
			return Ok(res);
		}

		public virtual async Task<IHttpActionResult> Post([FromUri] TKey id, TItem item)
		{
			TKey res = await Manager.Create(User, id, item);
			return Created(res.ToString(), item);
		}

		public virtual async Task<IHttpActionResult> Put([FromUri] TKey id, [FromUri] TKey childId, [FromBody] TItem item)
		{
			await Manager.Replace(User, id, childId, item);
			return NoContent();
		}

		public virtual async Task<IHttpActionResult> Delete([FromUri] TKey id, [FromUri] TKey childId)
		{
			await Manager.Delete(User, id, childId);
			return NoContent();
		}

		protected virtual Results<object> GetViewModel(Results<TItem> results)
		{
			return results.AsObjects<object>();
		}
	}
}