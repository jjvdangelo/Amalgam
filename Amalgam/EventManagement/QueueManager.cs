namespace Amalgam.EventManagement
{
	using System.Collections.Concurrent;
	using System.Diagnostics;
	using System.Threading;

	internal class QueueManager
	{
		private ConcurrentQueue<object> _events;

		public void RegisterEvent(params object[] events)
		{
			Debug.Assert(events != null);
			foreach (var e in events)
			{
				_events.Enqueue(e);
			}
		}

		public object[] GetEvents()
		{
			var events = Interlocked.Exchange(ref _events, new ConcurrentQueue<object>());
			return events.ToArray();
		}

		public QueueManager()
		{
			_events = new ConcurrentQueue<object>();
		}
	}
}