using System;

namespace PeterLeslieMorris.Correlation
{
	internal struct CorrelationIdData
	{
		public string Id { get; }

		public CorrelationIdData(string id)
		{
			Id = id;
		}
	}
}
