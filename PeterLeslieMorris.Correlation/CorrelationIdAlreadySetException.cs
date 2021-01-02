using System;

namespace PeterLeslieMorris.Correlation
{
	public class CorrelationIdAlreadySetException : Exception
	{
		public string ExistingValue { get; }
		public string NewValue { get; }

		public CorrelationIdAlreadySetException(string existingValue, string newValue)
			: base($"Cannot change {nameof(CorrelationId)}.{nameof(CorrelationId.Value)} from '{existingValue}' to '{newValue}'")
		{
			ExistingValue = existingValue;
			NewValue = newValue;
		}
	}
}
