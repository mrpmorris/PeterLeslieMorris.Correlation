using Serilog.Core;
using Serilog.Events;

namespace PeterLeslieMorris.Correlation.Logging.Serilog.Enrichers
{
	public class CorrelationIdEnricher : ILogEventEnricher
	{
		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			string correlationId = CorrelationId.Value;
			if (correlationId != null)
			{
				LogEventProperty property = propertyFactory.CreateProperty("CorrelationId", correlationId);
				logEvent.AddPropertyIfAbsent(property);
			}
		}
	}
}
