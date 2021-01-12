using Microsoft.Azure.ServiceBus;
using PeterLeslieMorris.Correlation.Helpers;
using System;
using System.Collections.Generic;

namespace PeterLeslieMorris.Correlation.ServiceBus.Listeners
{
	class ServiceBusDiagnosticEventObserver : IObserver<KeyValuePair<string, object>>
	{
		private static GetObjectPropertyValueDelegate<Message> GetMessagePropertyValue;
		private static GetObjectPropertyValueDelegate<IEnumerable<Message>> GetMessagesPropertyValue;

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
					// Set even if CorrelationId.HasValue, so we can throw an exception
					// if the value has changed
					GetCorrelationIdFromMessage(@event.Value);
					break;
			}
		}

		private static void GetCorrelationIdFromMessage(object eventData)
		{
			Message message = GetMessage(eventData);
			CorrelationId.Value = message.CorrelationId;
		}

		private static void SetMessagesCorrelationIds(object eventData)
		{
			var messages = GetMessages(eventData);
			foreach (Message message in messages)
				if (message.CorrelationId == null)
					message.CorrelationId = CorrelationId.Value;
		}

		private static Message GetMessage(object eventData)
		{
			if (GetMessagePropertyValue == null)
				GetMessagePropertyValue = GetObjectPropertyValueDelegateFactory.Create<Message>(eventData.GetType(), "Message");
			return GetMessagePropertyValue(eventData);
		}

		private static IEnumerable<Message> GetMessages(object eventData)
		{
			if (GetMessagesPropertyValue == null)
				GetMessagesPropertyValue = GetObjectPropertyValueDelegateFactory.Create<IEnumerable<Message>>(eventData.GetType(), "Messages");
			return GetMessagesPropertyValue(eventData);
		}
	}
}
