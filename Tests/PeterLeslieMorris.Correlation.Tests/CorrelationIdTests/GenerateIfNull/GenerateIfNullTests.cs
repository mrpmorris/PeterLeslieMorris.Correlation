using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PeterLeslieMorris.Correlation.Tests.CorrelationIdTests.GenerateIfNull
{
	public class GenerateIfNullTests
	{
		[Fact]
		public void WhenValueIsNull_ThenGeneratesANewValue()
		{
			Assert.Null(CorrelationId.Value);
			CorrelationId.GenerateIfNull();
			Assert.NotNull(CorrelationId.Value);
		}

		[Fact]
		public void WhenValueIsNotNull_ThenKeepsExistingValue()
		{
			string expectedId = "Expected";
			CorrelationId.Value = expectedId;
			CorrelationId.GenerateIfNull();
			Assert.Equal(expectedId, CorrelationId.Value);
		}
	}
}
