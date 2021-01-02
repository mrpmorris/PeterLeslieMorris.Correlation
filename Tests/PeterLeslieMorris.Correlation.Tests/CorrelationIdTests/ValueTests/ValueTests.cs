using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PeterLeslieMorris.Correlation.Tests.CorrelationIdTests.ValueTests
{
	public class ValueTests
	{
		[Fact]
		public void WhenReplacingNullValue_ThenValueIsSetCorrectly()
		{
			CorrelationId.Value = "Hello";
			Assert.Equal("Hello", CorrelationId.Value);
		}

		[Fact]
		public void WhenReplacingNonNullValueWithDifferentValue_ThenExceptionIsThrown()
		{
			CorrelationId.Value = "Hello";
			var exception = Assert.Throws<CorrelationIdAlreadySetException>(() => CorrelationId.Value = "Bye");
			Assert.Equal("Hello", exception.ExistingValue);
			Assert.Equal("Bye", exception.NewValue);
		}


		[Fact]
		public void WhenReplacingNonNullValueWithSameValue_ThenNoExceptionIsThrown()
		{
			CorrelationId.Value = "Hello";
			CorrelationId.Value = "Hello";
		}

		[Fact]
		public void WhenDifferentTasksAndThreadsSetValue_ThenShouldUseDifferentCorrelationIds()
		{
			int numberOfTasksExecuting = 0;

			var startSignal = new ManualResetEvent(false);
			Action testAction = new Action(() =>
			{
				string value = Guid.NewGuid().ToString();
				startSignal.WaitOne();
				CorrelationId.Value = value;
				Assert.Equal(value, CorrelationId.Value);
				Interlocked.Decrement(ref numberOfTasksExecuting);
			});

			foreach (int i in Enumerable.Range(1, 100))
			{
				Interlocked.Increment(ref numberOfTasksExecuting);
				Task.Run(testAction);
			}

			foreach(int i in Enumerable.Range(1, Environment.ProcessorCount))
			{
				Interlocked.Increment(ref numberOfTasksExecuting);
				var threadCode = new ThreadStart(testAction);
				new Thread(threadCode).Start();
			}

			startSignal.Set();
			while (numberOfTasksExecuting > 0)
				Thread.Sleep(50);
		}
	}
}
