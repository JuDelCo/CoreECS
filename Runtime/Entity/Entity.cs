using System.Collections.Generic;

namespace Ju.ECS
{
	public class Entity : IEntity
	{
		public event EntityComponentChangedEvent OnComponentAdded = delegate { };
		public event EntityComponentReplacedEvent OnComponentReplaced = delegate { };
		public event EntityComponentChangedEvent OnComponentRemoved = delegate { };

		private List<int> componentTypes;
		private int[] componentPoolIndices;
		private uint uuid;
		private static uint uuidGenerator = 0;

		public Entity(int componentTypeCount)
		{
			uuid = uuidGenerator++;
			componentTypes = new List<int>();
			componentPoolIndices = new int[componentTypeCount];

			for (int i = 0; i < componentTypeCount; ++i)
			{
				componentPoolIndices[i] = -1;
			}
		}

		public void AddComponent(int componentTypeId, int componentPoolIndex)
		{
			componentTypes.Add(componentTypeId);
			componentPoolIndices[componentTypeId] = componentPoolIndex;

			OnComponentAdded(this, componentTypeId);
		}

		public void ReplaceComponent(int componentTypeId)
		{
			OnComponentReplaced(this, componentTypeId);
		}

		public void RemoveComponent(int componentTypeId)
		{
			componentTypes.Remove(componentTypeId);
			componentPoolIndices[componentTypeId] = -1;

			OnComponentRemoved(this, componentTypeId);
		}

		public bool HasComponent(int componentTypeId)
		{
			return componentPoolIndices[componentTypeId] >= 0;
		}

		public int GetComponentPoolIndex(int componentTypeId)
		{
			return componentPoolIndices[componentTypeId];
		}

		public int GetTotalComponents()
		{
			return componentTypes.Count;
		}

		public void RemoveAllComponents()
		{
			for (int i = (componentTypes.Count - 1); i >= 0; --i)
			{
				RemoveComponent(componentTypes[i]);
			}
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
