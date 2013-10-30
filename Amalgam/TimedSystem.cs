namespace Amalgam
{
	using System;

	public abstract class TimedSystem : SystemBase
	{
		private readonly double _delay;
		private double _count;

		protected abstract void Frame();

		public override void Frame(double dt)
		{
			_count += dt;
			if (_count <= _delay)
			{
				Frame();
				_count -= _delay;
			}
		}

		protected TimedSystem(double seconds, EntityManager manager)
			: base(manager)
		{
			_count = seconds;
		}

		protected TimedSystem(TimeSpan delay, EntityManager manager)
			: base(manager)
		{
			_delay = delay.TotalSeconds;
		}
	}
}