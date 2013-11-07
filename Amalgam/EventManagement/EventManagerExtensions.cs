namespace Amalgam.EventManagement
{
	using System;

	public struct HandlerRegistration<T>
	{
		private readonly Action<object> _handler;

		public void Unregister()
		{
			EventManager.Instance.UnregisterFromEvent<T>(_handler);
		}

		public HandlerRegistration(Action<object> handler)
		{
			_handler = handler;
		}
	}

	public static class EventManagerExtensions
	{
		public static HandlerRegistration<T> RegisterToEvent<T>(this EventManager manager, Action<T> handler)
		{
			Action<object> temp = x =>
			{
				var e = (T)x;
				handler(e);
			};
			manager.RegisterToEvent<T>(temp);
			return new HandlerRegistration<T>(temp);
		}
	}
}