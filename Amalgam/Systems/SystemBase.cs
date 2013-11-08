namespace Amalgam.Systems
{
	using Amalgam.Entities;

	public abstract class SystemBase
	{
		private readonly EntityManager _manager;

		protected EntityManager EntityManager { get { return _manager; } }

		public virtual void Initialize() { }
		public abstract void Frame(double dt);
		public virtual void Shutdown() { }

		protected SystemBase(EntityManager manager)
		{
			_manager = manager;
		}
	}
}