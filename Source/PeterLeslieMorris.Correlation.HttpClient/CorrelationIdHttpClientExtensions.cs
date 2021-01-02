using PeterLeslieMorris.Correlation.HttpClient.Listeners;
using System.Diagnostics;

namespace PeterLeslieMorris.Correlation
{
	public static class CorrelationIdHttpClientExtensions
	{
		private static bool Installed;

		public static CorrelationId AddHttpClient(this CorrelationId instance)
		{
			if (!Installed)
				Install();
			return instance;
		}

		private static void Install()
		{
			Installed = true;
			DiagnosticListener.AllListeners.Subscribe(new HttpClientDiagnosticSourceObserver());
		}
	}
}
