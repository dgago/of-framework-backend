using of.support.configuration;
using of.support.diagnostics;

namespace of.support
{
	public static class Support
	{
		public static ILogProvider LogProvider { get; set; } = new SilentLogProvider();
		public static ISettingManager Settings { get; set; }
	}
}