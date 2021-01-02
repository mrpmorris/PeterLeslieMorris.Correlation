using System.Threading;

namespace PeterLeslieMorris.Correlation
{
	public static class CorrelationId
	{
		private readonly static AsyncLocal<string> AsyncLocalValue = new AsyncLocal<string>();

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
