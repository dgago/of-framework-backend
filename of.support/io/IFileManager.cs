using System.Security.Principal;
using System.Threading.Tasks;

using of.data;

namespace of.support.io
{
	public interface IFileManager : IManager<FileModel, string>
	{
		Task<string> GetDownloadUrlAsync(IPrincipal user, string id);
	}
}