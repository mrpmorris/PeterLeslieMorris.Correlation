using System;
using System.Diagnostics;

namespace PeterLeslieMorris.Correlation.AspNetCore.Http.Listeners
{
	internal class AspNetCoreDiagnosticSourceObserver : IObserver<DiagnosticListener>
	{
		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(DiagnosticListener listener)
		{
			if (listener.Name == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")
				listener.Subscribe(new AspNetCoreDiagnosticEventObserver());
		}
	}
}
