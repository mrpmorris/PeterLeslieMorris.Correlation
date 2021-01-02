using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PeterLeslieMorris.Correlation.ServiceBus.Listeners
{
	class ServiceBusDiagnosticEventObserver : IObserver<KeyValuePair<string, object>>
	{
		private delegate Message MessagePropertyGetterDelegate(object instance);
		private delegate Message[] MessagesPropertyGetterDelegate(object instance);

		private static MessagePropertyGetterDelegate MessagePropertyGetter;
		private static MessagesPropertyGetterDelegate MessagesPropertyGetter;

		public void OnCompleted() { }
		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> @event)
		{
			if (CorrelationId.Value != null)
			{
				switch (@event.Key)
				{
					case "Microsoft.Azure.ServiceBus.Send.Start":
						SetMessagesCorrelationIds(@event.Value);
						break;

					case "Microsoft.Azure.ServiceBus.Process.Start":
						GetCorrelationIdFromMessage(@event.Value);
						break;
				}
			}
		}

		private static void GetCorrelationIdFromMessage(object eventData)
		{
			Message message = GetMessage(eventData);
			if (message.CorrelationId != null)
				CorrelationId.Value = message.CorrelationId;
		}

		private static void SetMessagesCorrelationIds(object eventData)
		{
			var messages = GetMessages(eventData);
			foreach (Message message in messages)
				message.CorrelationId = message.CorrelationId ?? CorrelationId.Value;
		}

		private static Message GetMessage(object eventData)
		{
			if (MessagePropertyGetter == null)
				MessagePropertyGetter = CreateGetMessageFromPropertyDelegate(eventData.GetType());
			return MessagePropertyGetter(eventData);
		}

		private static MessagePropertyGetterDelegate CreateGetMessageFromPropertyDelegate(Type type)
		{
			MethodInfo propertyGetter = type.GetProperty("Message").GetGetMethod();
			return (MessagePropertyGetterDelegate)Delegate.CreateDelegate(type, propertyGetter);
		}

		private static Message[] GetMessages(object eventData)
		{
			if (MessagesPropertyGetter == null)
				MessagesPropertyGetter = CreateGetMessagesFromPropertyDelegate(eventData.GetType());
			return MessagesPropertyGetter(eventData);
		}

		private static MessagesPropertyGetterDelegate CreateGetMessagesFromPropertyDelegate(Type type)
		{
			MethodInfo propertyGetter = type.GetProperty("Messages").GetGetMethod();
			return (MessagesPropertyGetterDelegate)Delegate.CreateDelegate(type, propertyGetter);
		}
	}
}
