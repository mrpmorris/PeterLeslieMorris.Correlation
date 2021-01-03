namespace PeterLeslieMorris.Correlation
{
	internal struct CorrelationIdData
	{
		public string Id { get; }
		public bool ValueWasSetExplicitly { get; }

		public CorrelationIdData(string id, bool valueWasSetExplicitly)
		{
			Id = id;
			ValueWasSetExplicitly = valueWasSetExplicitly;
		}
	}
}
