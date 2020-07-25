using System;
using System.Collections.Generic;
using Ju.ECS.Internal;

namespace Ju.ECS
{
	public class Context : IContext
	{
		public event ContextEntityEvent OnEntityCreated;
		public event ContextEntityEvent OnEntityDestroyed;
		public event ContextGroupEvent OnGroupCreated;

		private static int contextIdGenerator = 0;

		private int contextId;
		private int componentTypeCount;
		private List<IEntity> entities;
		private Stack<IEntity> reausableEntities;
		private List<IEntity> retainedEntities;
		private Dictionary<int, List<IGroup>> groupsForType;
		private Dictionary<IMatcher, IGroup> groupCache;
		private Stack<List<GroupChangedEvent>> eventCache;
		private EntityComponentChangedEvent onEntityComponentAddedOrRemovedCache;
		private EntityComponentReplacedEvent onEntityComponentReplacedCache;
		private EntityEvent onEntityReleaseCache;
		private EntityEvent onEntityDestroyCache;
		private uint uuidGenerator = 0;
		private uint entityIdCounter = 0;

		public Context(int componentTypeCount) : this(componentTypeCount, 100000)
		{
			contextId = contextIdGenerator++;
			this.componentTypeCount = componentTypeCount;
			ComponentTypeIdsPerContext.SetContext(contextId, componentTypeCount);
		}

		public Context(int componentTypeCount, int capacity)
		{
			entities = new List<IEntity>(capacity);
			reausableEntities = new Stack<IEntity>(capacity);
			retainedEntities = new List<IEntity>(capacity);
			groupsForType = new Dictionary<int, List<IGroup>>(100);
			groupCache = new Dictionary<IMatcher, IGroup>(100);
			eventCache = new Stack<List<GroupChangedEvent>>();
			onEntityComponentAddedOrRemovedCache = OnEntityComponentAddedOrRemoved;
			onEntityComponentReplacedCache = OnEntityComponentReplaced;
			onEntityReleaseCache = OnEntityRelease;
			onEntityDestroyCache = OnEntityDestroy;
		}

		public IEntity CreateEntity()
		{
			IEntity entity;

			if (reausableEntities.Count > 0)
			{
				entity = reausableEntities.Pop();
				entity.Reactivate(entityIdCounter++);
			}
			else
			{
				entity = new Entity(contextId, componentTypeCount, uuidGenerator++);
			}

			entity.Retain();
			entities.Add(entity);

			entity.OnComponentAdded += onEntityComponentAddedOrRemovedCache;
			entity.OnComponentReplaced += onEntityComponentReplacedCache;
			entity.OnComponentRemoved += onEntityComponentAddedOrRemovedCache;
			entity.OnRelease += onEntityReleaseCache;
			entity.OnDestroy += onEntityDestroyCache;

			if (OnEntityCreated != null)
			{
				OnEntityCreated(this, entity);
			}

			return entity;
		}

		public List<IEntity> GetEntities()
		{
			return entities;
		}

		public IGroup GetGroup(IMatcher matcher)
		{
			IGroup group = null;

			if (groupCache.ContainsKey(matcher))
			{
				group = groupCache[matcher];
			}
			else
			{
				group = new Group(matcher);
				groupCache.Add(matcher, group);

				for (int i = 0, count = entities.Count; i < count; ++i)
				{
					group.HandleEntitySilently(entities[i]);
				}

				var types = matcher.GetTypes();

				for (int i = (types.Count - 1); i >= 0; --i)
				{
					if (!groupsForType.ContainsKey(types[i]))
					{
						groupsForType.Add(types[i], new List<IGroup>());
					}

					groupsForType[types[i]].Add(group);
				}

				if (OnGroupCreated != null)
				{
					OnGroupCreated(this, group);
				}
			}

			return group;
		}

		public int GetEntityCount()
		{
			return entities.Count;
		}

		public void DestroyAllEntities()
		{
			for (int i = (entities.Count - 1); i >= 0; --i)
			{
				entities[i].Destroy();
			}

			entities.Clear();
		}

		private void OnEntityComponentAddedOrRemoved(IEntity entity, int componentTypeId)
		{
			if (groupsForType.ContainsKey(componentTypeId))
			{
				var groups = groupsForType[componentTypeId];

				List<GroupChangedEvent> groupEvents;

				if (eventCache.Count != 0)
				{
					groupEvents = eventCache.Pop();
				}
				else
				{
					groupEvents = new List<GroupChangedEvent>(100);
				}

				for (int i = 0, count = groups.Count; i < count; ++i)
				{
					groupEvents.Add(groups[i].HandleEntity(entity));
				}

				for (int i = 0, count = groupEvents.Count; i < count; ++i)
				{
					if (groupEvents[i] != null)
					{
						groupEvents[i](groups[i], entity);
					}
				}

				groupEvents.Clear();
				eventCache.Push(groupEvents);
			}
		}

		private void OnEntityComponentReplaced(IEntity entity, int componentTypeId)
		{
			if (groupsForType.ContainsKey(componentTypeId))
			{
				var groups = groupsForType[componentTypeId];

				for (int i = 0, count = groups.Count; i < count; ++i)
				{
					groups[i].UpdateEntity(entity);
				}
			}
		}

		private void OnEntityRelease(IEntity entity)
		{
			retainedEntities.Remove(entity);
			reausableEntities.Push(entity);
		}

		private void OnEntityDestroy(IEntity entity)
		{
			if (!entities.Remove(entity))
			{
				throw new Exception("Context doesn't contain the entity");
			}

			entity.InternalDestroy();

			if (OnEntityDestroyed != null)
			{
				OnEntityDestroyed(this, entity);
			}

			if (entity.GetRetainCount() == 1)
			{
				entity.OnRelease -= onEntityReleaseCache;
				reausableEntities.Push(entity);
			}
			else
			{
				retainedEntities.Add(entity);
			}

			entity.Release();
		}
	}
}
