namespace Amalgam
{
	using System;
	using System.Globalization;

	public struct Entity : IEquatable<Entity>, IComparable<Entity>
	{
		readonly EntityManager _manager;
		readonly int _id;

		public TComponent GetComponent<TComponent>()
		{
			return _manager.GetComponentFromEntity<TComponent>(this);
		}

		public TComponent AddComponent<TComponent>(TComponent component)
		{
			return _manager.AddComponentToEntity(this, component);
		}

		public void RemoveComponent<TComponent>(TComponent component)
		{
			_manager.RemoveComponentFromEntity(this, component);
		}

		public void Destroy()
		{
			_manager.DestroyEntity(this);
		}

		public bool Equals(Entity other)
		{
			return ReferenceEquals(_manager, other._manager) &&
				   Equals(_id, other._id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return ReferenceEquals(this, obj) ||
				   obj is Entity ? Equals((Entity)obj) : false;
		}

		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}

		public override string ToString()
		{
			return "Entity-" + _id.ToString(CultureInfo.InvariantCulture);
		}

		public int CompareTo(Entity other)
		{
			return _id.CompareTo(other._id);
		}

		public static implicit operator int(Entity lhs)
		{
			return lhs._id;
		}

		public Entity(EntityManager manager, int id)
		{
			_manager = manager;
			_id = id;
		}
	}
}