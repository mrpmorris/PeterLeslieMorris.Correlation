using System;
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
			set => AsyncLocalValue.Value = value;
		}

		public static void GenerateIfNull()
		{
			if (Value == null)
				Value = Guid.NewGuid().ToString();
		}
	}
}
