using System;
using System.Collections.Generic;
using System.Reflection;

namespace PeterLeslieMorris.Correlation.ServiceBus.Listeners
{
	class HttpDiagnosticEventObserver : IObserver<KeyValuePair<string, object>>
	{
		public void OnCompleted() { }
		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> @event)
		{
			Console.WriteLine("Event: " + @event.Key);
		}
	}
}
