using System.Collections.Generic;

namespace Ju.ECS
{
	public class Entity : IEntity
	{
		public event EntityComponentChangedEvent OnComponentAdded;
		public event EntityComponentReplacedEvent OnComponentReplaced;
		public event EntityComponentChangedEvent OnComponentRemoved;
		public event EntityEvent OnRelease;
		public event EntityEvent OnDestroy;

		private bool isEnabled;
		private List<int> componentTypes;
		private int[] componentPoolIndices;
		private uint uuid;
		private static uint uuidGenerator = 0;
		private int retainCount = 0;

		public Entity(int componentTypeCount)
		{
			isEnabled = true;
			uuid = uuidGenerator++;
			componentTypes = new List<int>(componentTypeCount);
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

			if (OnComponentAdded != null)
			{
				OnComponentAdded(this, componentTypeId);
			}
		}

		public void ReplaceComponent(int componentTypeId)
		{
			if (OnComponentReplaced != null)
			{
				OnComponentReplaced(this, componentTypeId);
			}
		}

		public void RemoveComponent(int componentTypeId)
		{
			componentTypes.Remove(componentTypeId);
			componentPoolIndices[componentTypeId] = -1;

			if (OnComponentRemoved != null)
			{
				OnComponentRemoved(this, componentTypeId);
			}
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

		public bool IsEnabled()
		{
			return isEnabled;
		}

		public void Destroy()
		{
			if (OnDestroy != null)
			{
				OnDestroy(this);
			}
		}

		public void Retain()
		{
			retainCount++;
		}

		public void Release()
		{
			retainCount--;

			if (retainCount == 0)
			{
				if (OnRelease != null)
				{
					OnRelease(this);
					OnRelease = null;
				}
			}
		}

		public void Reactivate()
		{
			// TODO: Check
			//uuid = uuidGenerator++;
			isEnabled = true;
		}

		public int GetRetainCount()
		{
			return retainCount;
		}

		public void InternalDestroy()
		{
			isEnabled = false;

			RemoveAllComponents();

			OnComponentAdded = null;
			OnComponentReplaced = null;
			OnComponentRemoved = null;
			OnDestroy = null;
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
