using System.Threading;

namespace PeterLeslieMorris.Correlation
{
	public sealed class CorrelationId
	{
		public static CorrelationId Pipeline { get; }
		private readonly static AsyncLocal<string> AsyncLocalValue = new AsyncLocal<string>();

		private CorrelationId() { }

		public static bool HasValue => Value != null;

		public static string Value
		{
			get => AsyncLocalValue.Value;
			set
			{
				string existingValue = Value;
				if (existingValue != null && existingValue != value)
					throw new CorrelationIdAlreadySetException(existingValue: existingValue, newValue: value);
				AsyncLocalValue.Value = value;
			}
		}
	}
}
