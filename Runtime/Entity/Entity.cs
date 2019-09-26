using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Entity : IEntity
	{
		public event EntityComponentChangedEvent OnComponentAdded = delegate { };
		public event EntityComponentReplacedEvent OnComponentReplaced = delegate { };
		public event EntityComponentChangedEvent OnComponentRemoved = delegate { };

		private HashSet<int> componentTypes;
		private List<IComponent> components;
		private uint uuid;
		private static uint uuidGenerator = 0;

		public Entity()
		{
			componentTypes = new HashSet<int>();
			components = new List<IComponent>(100);
			uuid = uuidGenerator++;
		}

		public T CreateComponent<T>() where T : IComponent, new()
		{
			return new T();
		}

		public IEntity AddComponent(IComponent component)
		{
			var componentTypeId = component.GetTypeId();

			if (HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity already has a component of type {0}", component.GetType()));
			}

			componentTypes.Add(componentTypeId);
			SetComponent(componentTypeId, component);

			OnComponentAdded(this, component);

			return this;
		}

		public IEntity ReplaceComponent(IComponent component)
		{
			var componentTypeId = component.GetTypeId();

			if (HasComponent(componentTypeId))
			{
				var previousComponent = GetComponent(componentTypeId);

				if (previousComponent != component)
				{
					SetComponent(componentTypeId, component);
				}

				OnComponentReplaced(this, previousComponent, component);
			}
			else
			{
				AddComponent(component);
			}

			return this;
		}

		public IEntity RemoveComponent(int componentTypeId)
		{
			if (!HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity does not have a component of typeId {0}", componentTypeId));
			}

			var previousComponent = GetComponent(componentTypeId);

			componentTypes.Remove(componentTypeId);
			components.RemoveAt(componentTypeId);

			OnComponentRemoved(this, previousComponent);

			return this;
		}

		public bool HasComponent(int componentTypeId)
		{
			return (components.Count > componentTypeId && components[componentTypeId] != null);
		}

		public IComponent GetComponent(int componentTypeId)
		{
			if (!HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity does not have a component of typeId {0}", componentTypeId));
			}

			return components[componentTypeId];
		}

		public int GetTotalComponents()
		{
			return componentTypes.Count;
		}

		public void RemoveAllComponents()
		{
			foreach (var component in components)
			{
				OnComponentRemoved(this, component);
			}

			componentTypes.Clear();
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

		private void SetComponent(int componentTypeId, IComponent component)
		{
			if(componentTypeId > (components.Count - 1))
			{
				for (int i = components.Count - 1; i < componentTypeId; ++i)
				{
					components.Add(null);
				}
			}

			components[componentTypeId] = component;
		}
	}
}
