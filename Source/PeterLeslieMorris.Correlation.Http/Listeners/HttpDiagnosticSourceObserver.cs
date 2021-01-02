﻿using System;
using System.Diagnostics;

namespace PeterLeslieMorris.Correlation.ServiceBus.Listeners
{
	internal class HttpDiagnosticSourceObserver : IObserver<DiagnosticListener>
	{
		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(DiagnosticListener listener)
		{
			//if (listener.Name == "Microsoft.Azure.ServiceBus")
				listener.Subscribe(new HttpDiagnosticEventObserver());
		}
	}
}
