namespace Amalgam
{
	using System;
	using System.Linq;
	using System.Threading;

	public class SystemManager
	{
		private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
		private SystemBase[] _systems = new SystemBase[0];

		private SystemBase[] Systems { get { return _systems; } }

		public void RegisterSystem(SystemBase system)
		{
			RegisterSystem(new[] { system });
		}

		public void RegisterSystem(params SystemBase[] systems)
		{
			_lock.EnterWriteLock();

			try
			{
				var temp = new SystemBase[_systems.Length + systems.Length];
				Array.Copy(_systems, temp, _systems.Length);
				Array.Copy(systems, 0, temp, temp.Length - systems.Length, systems.Length);
				Interlocked.Exchange(ref _systems, temp);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public void UnregisterSystem(SystemBase system)
		{
			UnregisterSystem(new[] { system });
		}

		public void UnregisterSystem(params SystemBase[] systems)
		{
			_lock.EnterWriteLock();

			try
			{
				var list = _systems.ToList();
				foreach(var system in systems)
				{
					list.Remove(system);
				}

				var temp = new SystemBase[list.Count];
				Array.Copy(list.ToArray(), temp, list.Count);
				Interlocked.Exchange(ref _systems, temp);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public SystemBase[] UnregisterAll()
		{
			_lock.EnterWriteLock();

			try
			{
				var temp = new SystemBase[0];
				return Interlocked.Exchange(ref _systems, temp);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public void ForEach(Action<SystemBase> action)
		{
			foreach (var system in _systems)
			{
				action(system);
			}
		}
	}
}