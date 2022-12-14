using NLog;

namespace medirect_currency_exchange.Logger
{
	public class LoggerManager : ILoggerManager
	{
		private static ILogger _logger = LogManager.GetCurrentClassLogger();

		public void LogError(string message)
		{
			_logger.Error(message);
		}

		public void LogDebug(string message)
		{
			_logger.Debug(message);
		}

		public void LogInfo(string message)
		{
			_logger.Info(message);
		}
	}
}