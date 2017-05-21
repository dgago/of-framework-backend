using System;

namespace of.support.diagnostics
{
	public class SilentLogProvider : ILogProvider
	{
		public void Write(LogLevel type, string messageTemplate, params object[] propertyValues)
		{
		}

		public void Information(string messageTemplate, params object[] propertyValues)
		{
		}

		public void Warning(string messageTemplate, params object[] propertyValues)
		{
		}

		public void Error(string messageTemplate, params object[] propertyValues)
		{
		}

		public void Debug(string messageTemplate, params object[] propertyValues)
		{
		}

		public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
		{
		}

		public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
		{
		}

		public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
		{
		}

		public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
		{
		}
	}
}