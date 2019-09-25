using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Entity : IEntity
	{
		public event EntityComponentChangedEvent OnComponentAdded = delegate { };
		public event EntityComponentReplacedEvent OnComponentReplaced = delegate { };
		public event EntityComponentChangedEvent OnComponentRemoved = delegate { };

		private Dictionary<Type, IComponent> components;
		private uint uuid;
		private static uint uuidGenerator = 0;

		public Entity()
		{
			components = new Dictionary<Type, IComponent>(10);
			uuid = uuidGenerator++;
		}

		public T CreateComponent<T>() where T : IComponent, new()
		{
			return new T();
		}

		public IEntity AddComponent(IComponent component)
		{
			var type = component.GetType();

			if (HasComponent(type))
			{
				throw new Exception(string.Format("Entity already has a component of type {0}", type));
			}

			components.Add(type, component);

			OnComponentAdded(this, component);

			return this;
		}

		public IEntity ReplaceComponent(IComponent component)
		{
			var type = component.GetType();

			if (HasComponent(type))
			{
				var previousComponent = GetComponent(type);

				if (previousComponent != component)
				{
					components.Remove(type);
					components.Add(type, component);
				}

				OnComponentReplaced(this, previousComponent, component);
			}
			else
			{
				AddComponent(component);
			}

			return this;
		}

		public IEntity RemoveComponent(Type type)
		{
			if (!HasComponent(type))
			{
				throw new Exception(string.Format("Entity does not have a component of type {0}", type));
			}

			var previousComponent = GetComponent(type);

			components.Remove(type);

			OnComponentRemoved(this, previousComponent);

			return this;
		}

		public bool HasComponent(Type type)
		{
			return components.ContainsKey(type);
		}

		public IComponent GetComponent(Type type)
		{
			if (!HasComponent(type))
			{
				throw new Exception(string.Format("Entity does not have a component of type {0}", type));
			}

			return components[type];
		}

		public int GetTotalComponents()
		{
			return components.Count;
		}

		public void RemoveAllComponents()
		{
			foreach (var kvp in components)
			{
				OnComponentRemoved(this, kvp.Value);
			}

			components.Clear();
		}

		public uint GetUuid()
		{
			return uuid;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType() || obj.GetHashCode() != GetHashCode())
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return (int)GetUuid();
		}
	}
}
