namespace Amalgam.EventManagement
{
	using System;
	using System.Diagnostics;

	public interface IEventRegistrar
	{
		void RegisterToEvent<T>(Action<object> handler);
		void UnregisterFromEvent<T>(Action<object> handler);
	}

	public interface IEventNotifier
	{
		void Notify(object e);
		void Notify(params object[] e);
	}

	public sealed class EventManager : IEventRegistrar, IEventNotifier
	{
		private static readonly EventManager _instance = new EventManager();
		public static EventManager Instance { get { return _instance; } }

		private readonly HandlerRegistrar _registrar;
		private readonly QueueManager _queue;

		public void RegisterToEvent<T>(Action<object> handler)
		{
			_registrar.RegisterHandlerToEvent(typeof(T), handler.ThrowIfNull("handler"));
		}

		public void UnregisterFromEvent<T>(Action<object> handler)
		{
			_registrar.UnregisterHandlerFromEvent(typeof(T), handler.ThrowIfNull("handler"));
		}

		public void Notify(object e)
		{
			Notify(new[] { e.ThrowIfNull("e") });
		}

		public void Notify(params object[] e)
		{
			_queue.RegisterEvent(e.ThrowIfNull("e"));
		}

		public void FireEvents()
		{
			foreach(var e in _queue.GetEvents())
			{
				_registrar.GetHandlersForEvent(e.GetType(), x => x(e), GetHandlerNotFound(e.GetType()));
			}
		}

		private static Action GetHandlerNotFound(Type t)
		{
			return () =>
			{
				var error = string.Format("No handler found for event: {0}.", t);
				Trace.WriteLine(error, "Alert");
			};
		}

		private EventManager()
		{
			_registrar = new HandlerRegistrar();
			_queue = new QueueManager();
		}

		static EventManager() { }
	}
}