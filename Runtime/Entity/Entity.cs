using System.Collections.Generic;
using Ju.ECS.Internal;

namespace Ju.ECS
{
	public class Entity : IEntity
	{
		public event EntityComponentChangedEvent OnComponentAdded;
		public event EntityComponentReplacedEvent OnComponentReplaced;
		public event EntityComponentChangedEvent OnComponentRemoved;
		public event EntityEvent OnRelease;
		public event EntityEvent OnDestroy;

		private readonly int contextId;
		private readonly List<int> componentTypes;
		private readonly int[] componentPoolIndices;
		private readonly Stack<int>[] componentUnusedIndices;
		private readonly uint uuid;
		private bool isEnabled;
		private uint entityId;
		private int retainCount = 0;

		public Entity(int contextId, int componentTypeCount, uint entityId)
		{
			this.contextId = contextId;

			uuid = entityId;
			Reactivate(entityId);

			componentTypes = new List<int>(componentTypeCount);
			componentPoolIndices = new int[componentTypeCount];
			componentUnusedIndices = new Stack<int>[componentTypeCount];

			for (int i = 0; i < componentTypeCount; ++i)
			{
				componentPoolIndices[i] = -1;
			}
		}

		public void AddComponent(int componentTypeId, int componentPoolIndex, Stack<int> unusedIndicesRef)
		{
			var contextComponentTypeId = GetContextComponentTypeId(componentTypeId);

			componentTypes.Add(componentTypeId);
			componentPoolIndices[contextComponentTypeId] = componentPoolIndex;
			componentUnusedIndices[contextComponentTypeId] = unusedIndicesRef;

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
			var contextComponentTypeId = GetContextComponentTypeId(componentTypeId);

			componentTypes.Remove(componentTypeId);

			componentUnusedIndices[contextComponentTypeId].Push(componentPoolIndices[contextComponentTypeId]);
			componentUnusedIndices[contextComponentTypeId] = null;
			componentPoolIndices[contextComponentTypeId] = -1;

			if (OnComponentRemoved != null)
			{
				OnComponentRemoved(this, componentTypeId);
			}
		}

		public bool HasComponent(int componentTypeId)
		{
			return componentPoolIndices[GetContextComponentTypeId(componentTypeId)] >= 0;
		}

		public int GetComponentPoolIndex(int componentTypeId)
		{
			return componentPoolIndices[GetContextComponentTypeId(componentTypeId)];
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

		public uint GetEntityId()
		{
			return entityId;
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

		public void Reactivate(uint entityId)
		{
			this.entityId = entityId;
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

		private int GetContextComponentTypeId(int componentTypeId)
		{
			var index = ComponentTypeIdsPerContext.GetTypeId(contextId, componentTypeId);

			if (index == -1)
			{
				throw new System.Exception("Entity can't handle more component types");
			}

			return index;
		}
	}
}
