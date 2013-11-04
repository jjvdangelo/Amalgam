namespace Amalgam.EventManagement
{
	using System;
	using System.Collections.Concurrent;
	using System.Diagnostics;

	internal class HandlerRegistrar
	{
		private readonly ConcurrentDictionary<Type, Action<object>> _handlers;

		public void RegisterHandlerToEvent(Type eventType, Action<object> handler)
		{
			Debug.Assert(eventType != null);
			Debug.Assert(handler != null);
			_handlers.AddOrUpdate(eventType, x =>
			{
				Action<object> temp = y => { };
				return temp += handler;
			}, (x, y) => y += handler);
		}

		public void UnregisterHandlerFromEvent(Type eventType, Action<object> handler)
		{
			Debug.Assert(eventType != null);
			Debug.Assert(handler != null);
			_handlers.AddOrUpdate(eventType, x => { }, (x, y) => y -= handler);
		}

		public void GetHandlersForEvent(Type eventType, Action<Action<object>> found, Action notFound)
		{
			Action<object> handlers;
			if (_handlers.TryGetValue(eventType, out handlers))
			{
				found(handlers);
			}
			else
			{
				notFound();
			}
		}

		public HandlerRegistrar()
		{
			_handlers = new ConcurrentDictionary<Type,Action<object>>();
		}
	}
}