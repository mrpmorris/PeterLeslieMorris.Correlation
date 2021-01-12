using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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
			if (@event.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")
				SetRequestCorrelationId((HttpContext)@event.Value);
		}

		private void SetRequestCorrelationId(HttpContext context)
		{
			HttpRequest request = context.Request;
			if (request.Headers.TryGetValue(XCorrelationIdHeaderName, out StringValues id))
				CorrelationId.Value = id;
			else
				CorrelationId.Value = null;
		}
	}
}
