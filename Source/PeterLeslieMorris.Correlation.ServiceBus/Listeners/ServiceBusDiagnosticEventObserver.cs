using Microsoft.Azure.ServiceBus;
using PeterLeslieMorris.Correlation.Helpers;
using System;
using System.Collections.Generic;

namespace PeterLeslieMorris.Correlation.ServiceBus.Listeners
{
	class ServiceBusDiagnosticEventObserver : IObserver<KeyValuePair<string, object>>
	{
		private static GetObjectPropertyValueDelegate<Message> GetMessagePropertyValueFromProcessStart;
		private static GetObjectPropertyValueDelegate<Message> GetMessagePropertyValueFromProcessSessionStart;
		private static GetObjectPropertyValueDelegate<IEnumerable<Message>> GetMessagesPropertyValueFromSendStart;

		public void OnCompleted() { }
		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> @event)
		{
			switch (@event.Key)
			{
				case "Microsoft.Azure.ServiceBus.Send.Start":
						SetMessagesCorrelationIds(@event.Value);
					break;

				case "Microsoft.Azure.ServiceBus.Process.Start":
					GetCorrelationIdFromProcessStartMessage(@event.Value);
					break;

				case "Microsoft.Azure.ServiceBus.ProcessSession.Start":
					GetCorrelationIdFromProcessSessionStartMessage(@event.Value);
					break;
			}
		}

		private static void GetCorrelationIdFromProcessStartMessage(object eventData)
		{
			Message message = GetMessageFromProcessStart(eventData);
			CorrelationId.Value = message.CorrelationId;
		}

		private static void GetCorrelationIdFromProcessSessionStartMessage(object eventData)
		{
			Message message = GetMessageFromProcessSessionStart(eventData);
			CorrelationId.Value = message.CorrelationId;
		}

		private static void SetMessagesCorrelationIds(object eventData)
		{
			var messages = GetMessagesFromSendStart(eventData);
			foreach (Message message in messages)
				if (message.CorrelationId == null)
					message.CorrelationId = CorrelationId.Value;
		}

		private static Message GetMessageFromProcessStart(object eventData)
		{
			if (GetMessagePropertyValueFromProcessStart == null)
				GetMessagePropertyValueFromProcessStart = GetObjectPropertyValueDelegateFactory.Create<Message>(eventData.GetType(), "Message");
			return GetMessagePropertyValueFromProcessStart(eventData);
		}

		private static Message GetMessageFromProcessSessionStart(object eventData)
		{
			if (GetMessagePropertyValueFromProcessSessionStart == null)
				GetMessagePropertyValueFromProcessSessionStart = GetObjectPropertyValueDelegateFactory.Create<Message>(eventData.GetType(), "Message");
			return GetMessagePropertyValueFromProcessSessionStart(eventData);
		}

		private static IEnumerable<Message> GetMessagesFromSendStart(object eventData)
		{
			if (GetMessagesPropertyValueFromSendStart == null)
				GetMessagesPropertyValueFromSendStart = GetObjectPropertyValueDelegateFactory.Create<IEnumerable<Message>>(eventData.GetType(), "Messages");
			return GetMessagesPropertyValueFromSendStart(eventData);
		}
	}
}
