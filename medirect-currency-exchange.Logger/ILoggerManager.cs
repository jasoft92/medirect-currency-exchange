namespace medirect_currency_exchange.Logger
{
	public interface ILoggerManager
	{
		void LogError(string message);
		void LogDebug(string message);
		void LogInfo(string message);
	}
}
