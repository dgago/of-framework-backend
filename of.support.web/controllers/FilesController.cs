using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using of.support.io;
using of.web.http;

namespace of.support.web.controllers
{
	public class FilesController : BaseApiController
	{
		private readonly string _workingFolder = Path.Combine(HttpRuntime.AppDomainAppPath, "temp", "u");
		private readonly IFileManager _manager;

		public FilesController(IFileManager manager)
		{
			_manager = manager;
		}

		[HttpPost]
		public async Task<IHttpActionResult> Post()
		{
			// Check if the request contains multipart/form-data.
			if (!Request.Content.IsMimeMultipartContent("form-data"))
			{
				return BadRequest("Unsupported media type");
			}

			CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(_workingFolder);

			//await Request.Content.ReadAsMultipartAsync(provider);
			await Task.Run(async () => await Request.Content.ReadAsMultipartAsync(provider));

			List<string> files = new List<string>();
			foreach (MultipartFileData file in provider.FileData)
			{
				FileInfo fileInfo = new FileInfo(file.LocalFileName);

				FileModel filem = new FileModel
				{
					Name = fileInfo.Name,
					Contents = File.ReadAllBytes(file.LocalFileName),
					Size = fileInfo.Length
				};
				string id = await _manager.CreateAsync(User, filem);

				files.Add(id);
			}

			return Created(files.First(), files);
		}
	}
}