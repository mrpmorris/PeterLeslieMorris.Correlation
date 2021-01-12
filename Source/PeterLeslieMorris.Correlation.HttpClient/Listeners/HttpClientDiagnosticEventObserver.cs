using PeterLeslieMorris.Correlation.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PeterLeslieMorris.Correlation.HttpClient.Listeners
{
	class HttpClientDiagnosticEventObserver : IObserver<KeyValuePair<string, object>>
	{
		private const string XCorrelationIdHeaderName = "X-Correlation-ID";
		private static GetObjectPropertyValueDelegate<HttpRequestMessage> GetRequestPropertyValue;

		public void OnCompleted() { }
		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> @event)
		{
			if (@event.Key == "System.Net.Http.HttpRequestOut.Start")
				SetRequestCorrelationId(@event.Value);
		}

		private void SetRequestCorrelationId(object eventData)
		{
			HttpRequestMessage requestMessage = GetRequest(eventData);
			if (!requestMessage.Headers.Contains(XCorrelationIdHeaderName))
				requestMessage.Headers.Add(XCorrelationIdHeaderName, CorrelationId.Value);
		}

		private static HttpRequestMessage GetRequest(object eventData)
		{
			if (GetRequestPropertyValue == null)
				GetRequestPropertyValue = GetObjectPropertyValueDelegateFactory.Create<HttpRequestMessage>(eventData.GetType(), "Request");
			return GetRequestPropertyValue(eventData);
		}
	}
}
