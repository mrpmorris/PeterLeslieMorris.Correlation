using System;
using System.Threading;

namespace PeterLeslieMorris.Correlation
{
	public sealed class CorrelationId
	{
		public static CorrelationId Pipeline { get; }
		private readonly static AsyncLocal<CorrelationIdData> Data = new AsyncLocal<CorrelationIdData>();

		private CorrelationId() { }

		public static string Value
		{
			get
			{
				// If value was not explicitly set to null, then ensure result is not null
				if (NeedsGeneratedDefaultValue(Data.Value.Id))
					GenerateUniqueDefaultValue();
				return Data.Value.Id;
			}
			set
			{
				if (NeedsGeneratedDefaultValue(value))
					GenerateUniqueDefaultValue();
				else
					Data.Value = new CorrelationIdData(value);
			}
		}

		private static void GenerateUniqueDefaultValue()
		{
			Data.Value = new CorrelationIdData(Guid.NewGuid().ToString());
		}

		private static bool NeedsGeneratedDefaultValue(string value)
			=> string.IsNullOrEmpty(value);
	}
}
