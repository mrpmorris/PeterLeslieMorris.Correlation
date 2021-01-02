using PeterLeslieMorris.Correlation.ServiceBus.Listeners;
using System.Diagnostics;

namespace PeterLeslieMorris.Correlation.ServiceBus
{
	public static class CorrelationIdHttpExtensions
	{
		private static bool Installed;

		public static CorrelationId FromHttp(this CorrelationId instance)
		{
			if (!Installed)
				Install();
			return instance;
		}

		private static void Install()
		{
			Installed = true;
			DiagnosticListener.AllListeners.Subscribe(new HttpDiagnosticSourceObserver());
		}
	}
}
