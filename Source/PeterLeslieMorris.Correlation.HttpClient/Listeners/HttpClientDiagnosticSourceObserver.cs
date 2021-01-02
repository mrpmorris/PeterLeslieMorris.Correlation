using System;
using System.Diagnostics;

namespace PeterLeslieMorris.Correlation.HttpClient.Listeners
{
	internal class HttpClientDiagnosticSourceObserver : IObserver<DiagnosticListener>
	{
		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(DiagnosticListener listener)
		{
			if (listener.Name == "HttpHandlerDiagnosticListener")
				listener.Subscribe(new HttpClientDiagnosticEventObserver());
		}
	}
}
