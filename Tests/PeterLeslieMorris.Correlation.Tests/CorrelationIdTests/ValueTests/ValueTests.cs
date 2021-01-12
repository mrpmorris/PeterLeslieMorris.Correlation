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
		public void WhenReplacingUniqueValueWithNull_ThenSetsValueToNewGuid()
		{
			string originalId = CorrelationId.Value;
			CorrelationId.Value = null;
			Assert.NotNull(CorrelationId.Value);
			Assert.NotEqual(originalId, CorrelationId.Value);
		}

		[Fact]
		public void WhenNoValueHasBeenSet_ThenReturnsDefaultUniqueValue()
		{
			Assert.NotNull(CorrelationId.Value);
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

		[Fact]
		public async Task WhenAwaitingMultipleTasks_ThenTheSameCorrelationIdIsUsed()
		{
			string expectedId = "TheValue";
			CorrelationId.Value = expectedId;

			async Task testCode()
			{
				await Task.Yield();
				Assert.Equal(expectedId, CorrelationId.Value);
			}

			Task task1 = testCode();
			Task task2 = testCode();

			await Task.WhenAll(task1, task2).ConfigureAwait(false);
		}
	}
}
