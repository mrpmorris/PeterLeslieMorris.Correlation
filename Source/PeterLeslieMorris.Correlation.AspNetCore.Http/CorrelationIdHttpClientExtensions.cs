using PeterLeslieMorris.Correlation.AspNetCore.Http.Listeners;
using System.Diagnostics;

namespace PeterLeslieMorris.Correlation
{
	public static class CorrelationIdHttpClientExtensions
	{
		private static bool Installed;

		public static CorrelationId AddAspNetCoreHttp(this CorrelationId instance)
		{
			if (!Installed)
				Install();
			return instance;
		}

		private static void Install()
		{
			Installed = true;
			DiagnosticListener.AllListeners.Subscribe(new AspNetCoreDiagnosticSourceObserver());
		}
	}
}
