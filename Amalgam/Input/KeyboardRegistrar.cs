namespace Amalgam.Input
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Windows.Forms;

	internal class KeyboardRegistrar
	{
		private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
		private Keyboard[] _keyboards = new Keyboard[0];
		private Dictionary<Keys, Action> _actions;

		public Keyboard[] Keyboard { get { return _keyboards; } }

		public void RegisterKeyboard(Keyboard keyboard)
		{
			_lock.EnterWriteLock();

			try
			{
				if (_keyboards.Contains(keyboard)) return;

				var temp = new Keyboard[_keyboards.Length + 1];
				Array.Copy(_keyboards, temp, _keyboards.Length);
				temp[_keyboards.Length] = keyboard;
				Interlocked.Exchange(ref _keyboards, temp);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public void UnregisterKeyboard(Keyboard keyboard)
		{
			_lock.EnterWriteLock();

			try
			{
				if (!_keyboards.Contains(keyboard)) return;

				var temp = _keyboards.ToList();
				temp.Remove(keyboard);
				Interlocked.Exchange(ref _keyboards, temp.ToArray());
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public KeyboardRegistrar()
		{
			var temp = Enum.GetValues(typeof(Keys))
						   .Cast<Keys>()
						   .Distinct()
						   .ToDictionary<Keys, Keys, Action>(x => x, x => () => { });
		}
	}
}