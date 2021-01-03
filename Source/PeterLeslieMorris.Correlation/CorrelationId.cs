using System;
using System.Threading;

namespace PeterLeslieMorris.Correlation
{
	public sealed class CorrelationId
	{
		public static CorrelationId Pipeline { get; }
		private readonly static AsyncLocal<CorrelationIdData> Data = new AsyncLocal<CorrelationIdData>();

		private CorrelationId() { }

		public static bool HasValue => Value != null;

		public static string Value
		{
			get
			{
				// If value was not explicitly set to null, then ensure result is not null
				GenerateUniqueDefaultValue();
				return Data.Value.Id;
			}
			set
			{
				Data.Value = new CorrelationIdData(id: value, valueWasSetExplicitly: true);
			}
		}

		private static void GenerateUniqueDefaultValue()
		{
			CorrelationIdData data = Data.Value;
			if (data.Id == null && !data.ValueWasSetExplicitly)
				Data.Value = new CorrelationIdData(id: Guid.NewGuid().ToString(), valueWasSetExplicitly: false);
		}
	}
}
