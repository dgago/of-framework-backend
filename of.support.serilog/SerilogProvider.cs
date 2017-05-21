using System;

using of.support.diagnostics;

using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace of.support.serilog
{
	public class SeriLogProvider : ILogProvider
	{
		private static Logger Log { get; } = new LoggerConfiguration()
			// .WriteTo.RollingFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log-{Date}.txt"), shared: true)
			// <add key="serilog:using:RollingFile" value="Serilog.Sinks.RollingFile" />
			// <add key="serilog:write-to:RollingFile.pathFormat" value="C:\Logs\log-{Date}.txt" />
			.ReadFrom.AppSettings()
			.CreateLogger();


		public void Write(LogLevel type, string messageTemplate, params object[] propertyValues)
		{
			Log.Write(GetLogEventLevel(type), messageTemplate, propertyValues);
		}

		public void Information(string messageTemplate, params object[] propertyValues)
		{
			Log.Information(messageTemplate, propertyValues);
		}

		public void Warning(string messageTemplate, params object[] propertyValues)
		{
			Log.Warning(messageTemplate, propertyValues);
		}

		public void Error(string messageTemplate, params object[] propertyValues)
		{
			Log.Error(messageTemplate, propertyValues);
		}

		public void Debug(string messageTemplate, params object[] propertyValues)
		{
			Log.Debug(messageTemplate, propertyValues);
		}

		public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Log.Information(exception, messageTemplate, propertyValues);
		}

		public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Log.Warning(exception, messageTemplate, propertyValues);
		}

		public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Log.Error(exception, messageTemplate, propertyValues);
		}

		public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Log.Debug(exception, messageTemplate, propertyValues);
		}

		#region helpers

		private LogEventLevel GetLogEventLevel(LogLevel type)
		{
			LogEventLevel res;
			switch (type)
			{
			case LogLevel.Error:
				res = LogEventLevel.Error;
				break;
			case LogLevel.Warning:
				res = LogEventLevel.Warning;
				break;
			case LogLevel.Information:
				res = LogEventLevel.Information;
				break;
			case LogLevel.Debug:
				res = LogEventLevel.Debug;
				break;
			case LogLevel.Verbose:
				res = LogEventLevel.Verbose;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}

			return res;
		}

		#endregion
	}
}