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
		private readonly IChildManager<TItem, TKey> _manager;

		public ChildApiController(IChildManager<TItem, TKey> manager)
		{
			_manager = manager;
		}

		[PaginationFilter]
		public async Task<IHttpActionResult> GetAll([FromUri] TKey id, [FromUri] int? pageIndex = null, [FromUri] int? pageSize = null)
		{
			IEnumerable<KeyValuePair<string, string>> qs = Request.GetQueryNameValuePairs();
			Results<TItem> results = await _manager.Find(User, id, qs, pageIndex ?? 1, pageSize ?? MAX_RECORDS);
			return OkCount(results);
		}

		public async Task<IHttpActionResult> GetOne([FromUri] TKey id, [FromUri] TKey childId)
		{
			TItem res = await _manager.FindOne(User, id, childId);
			if (res == null)
			{
				return NotFound();
			}
			return Ok(res);
		}

		public async Task<IHttpActionResult> Post([FromUri] TKey id, TItem item)
		{
			TKey res = await _manager.Create(User, id, item);
			return Created(res.ToString(), item);
		}

		public async Task<IHttpActionResult> Put([FromUri] TKey id, [FromUri] TKey childId, [FromBody] TItem item)
		{
			await _manager.Replace(User, id, childId, item);
			return NoContent();
		}

		public async Task<IHttpActionResult> Delete([FromUri] TKey id, [FromUri] TKey childId)
		{
			await _manager.Delete(User, id, childId);
			return NoContent();
		}
	}
}