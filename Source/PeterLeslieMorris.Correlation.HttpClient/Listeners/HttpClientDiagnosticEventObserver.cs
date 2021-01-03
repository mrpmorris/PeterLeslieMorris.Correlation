using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;

namespace PeterLeslieMorris.Correlation.HttpClient.Listeners
{
	class HttpClientDiagnosticEventObserver : IObserver<KeyValuePair<string, object>>
	{
		private const string XCorrelationId = "X-Correlation-ID";
		private delegate HttpRequestMessage RequestPropertyGetterDelegate(object instance);
		private static RequestPropertyGetterDelegate RequestPropertyGetter;

		public void OnCompleted() { }
		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> @event)
		{
			if (@event.Key == "System.Net.Http.HttpRequestOut.Start" && CorrelationId.HasValue)
				SetRequestCorrelationId(@event.Value);
		}

		private void SetRequestCorrelationId(object eventData)
		{
			HttpRequestMessage requestMessage = GetRequest(eventData);
			if (!requestMessage.Headers.Contains(XCorrelationId))
				requestMessage.Headers.Add(XCorrelationId, CorrelationId.Value);
		}

		private static HttpRequestMessage GetRequest(object eventData)
		{
			if (RequestPropertyGetter == null)
				RequestPropertyGetter = CreateRequestFromPropertyDelegate(eventData.GetType());
			return RequestPropertyGetter(eventData);
		}

		private static RequestPropertyGetterDelegate CreateRequestFromPropertyDelegate(Type type)
		{
			MethodInfo propertyGetter = type.GetProperty("Request").GetGetMethod();
			return (RequestPropertyGetterDelegate)Delegate.CreateDelegate(type, propertyGetter);
		}
	}
}
