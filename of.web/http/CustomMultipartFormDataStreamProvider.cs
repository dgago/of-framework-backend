using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace of.web.http
{
	public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
	{
		public CustomMultipartFormDataStreamProvider(string rootPath) : base(rootPath)
		{
		}

		public CustomMultipartFormDataStreamProvider(string rootPath, int bufferSize) : base(rootPath, bufferSize)
		{
		}

		public override string GetLocalFileName(HttpContentHeaders headers)
		{
			//Make the file name URL safe and then use it & is the only disallowed url character allowed in a windows filename
			string name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName)
								? headers.ContentDisposition.FileName
								: DateTime.UtcNow.Ticks.ToString();
			return name.Trim('"').Replace("&", "and");
		}
	}
}