namespace Amalgam
{
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	
	public class EntityManager
    {
		private int _nextId;
		private readonly ConcurrentDictionary<int, HashSet<object>> _entityComponents =
			new ConcurrentDictionary<int, HashSet<object>>();

		public Entity CreateEntity()
		{
			var id = Interlocked.Increment(ref _nextId);
			_entityComponents.TryAdd(id, new HashSet<object>());
			
			return new Entity(this, id);
		}

		public T AddComponentToEntity<T>(Entity entity, T component)
		{
			_entityComponents[entity].Add(component);
			return component;
		}

		public void RemoveComponentFromEntity<T>(Entity entity, T component)
		{
			_entityComponents.AddOrUpdate(entity,
				new HashSet<object>() { component },
				(e, hash) => { hash.Add(component); return hash; });
		}

		public T GetComponentFromEntity<T>(Entity entity)
		{
			return _entityComponents[entity].OfType<T>().FirstOrDefault();
		}

		public bool DestroyEntity(Entity entity)
		{
			HashSet<object> temp;
			return _entityComponents.TryRemove(entity, out temp);
		}

		public IEnumerable<Entity> GetEntitiesWithComponent<TComponent>()
		{
			return _entityComponents.Where(x => x.Value.OfType<TComponent>().Any())
									.Select(x => new Entity(this, x.Key));
		}
    }
}