using PeterLeslieMorris.Correlation.ServiceBus.Listeners;
using System.Diagnostics;

namespace PeterLeslieMorris.Correlation.ServiceBus
{
	public static class CorrelationIdServiceBusExtensions
	{
		private static bool Installed;

		public static CorrelationId FromServiceBus(this CorrelationId instance)
		{
			if (!Installed)
				Install();
			return instance;
		}

		private static void Install()
		{
			Installed = true;
			DiagnosticListener.AllListeners.Subscribe(new ServiceBusDiagnosticSourceObserver());
		}
	}
}
