using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace PeterLeslieMorris.Correlation.AspNetCore.Http.Listeners
{
	class AspNetCoreDiagnosticEventObserver : IObserver<KeyValuePair<string, object>>
	{
		private const string XCorrelationIdHeaderName = "X-Correlation-ID";

		public void OnCompleted() { }
		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> @event)
		{
			if (@event.Key == "System.Net.Http.HttpRequestOut.Start" && CorrelationId.HasValue)
				SetRequestCorrelationId((HttpContext)@event.Value);
		}

		private void SetRequestCorrelationId(HttpContext context)
		{
			HttpRequest request = context.Request;
			if (!request.Headers.ContainsKey(XCorrelationIdHeaderName))
				request.Headers.Add(XCorrelationIdHeaderName, CorrelationId.Value);
		}
	}
}
