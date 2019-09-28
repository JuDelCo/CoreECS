using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Context : IContext
	{
		public event ContextEntityEvent OnEntityCreated = delegate { };
		public event ContextEntityEvent OnEntityDestroyed = delegate { };
		public event ContextGroupEvent OnGroupCreated = delegate { };

		private int componentTypeCount;
		private List<IEntity> entities;
		private Dictionary<int, List<IGroup>> groupsForType;
		private Dictionary<IMatcher, IGroup> groupCache;

		public Context(int componentTypeCount) : this(componentTypeCount, 100000)
		{
			this.componentTypeCount = componentTypeCount;
		}

		public Context(int componentTypeCount, int capacity)
		{
			entities = new List<IEntity>(capacity);
			groupsForType = new Dictionary<int, List<IGroup>>(100);
			groupCache = new Dictionary<IMatcher, IGroup>();
		}

		public IEntity CreateEntity()
		{
			// TODO: Cache (Reuse destroyed entities)
			var entity = new Entity(componentTypeCount);
			entities.Add(entity);

			entity.OnComponentAdded += OnEntityComponentAddedOrRemoved;
			entity.OnComponentReplaced += OnEntityComponentReplaced;
			entity.OnComponentRemoved += OnEntityComponentAddedOrRemoved;

			OnEntityCreated(this, entity);

			return entity;
		}

		public void DestroyEntity(IEntity entity)
		{
			if (!entities.Remove(entity))
			{
				throw new Exception("Context does not contain the entity");
			}

			// TODO: Handle this block in Entity class
			entity.RemoveAllComponents();
			entity.OnComponentAdded -= OnEntityComponentAddedOrRemoved;
			entity.OnComponentReplaced -= OnEntityComponentReplaced;
			entity.OnComponentRemoved -= OnEntityComponentAddedOrRemoved;

			OnEntityDestroyed(this, entity);
		}

		public void DestroyAllEntities()
		{
			for (int i = (entities.Count - 1); i >= 0; --i)
			{
				DestroyEntity(entities[i]);
			}
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
			}

			for (int i = 0; i < entities.Count; ++i)
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

			OnGroupCreated(this, group);

			return group;
		}

		public int GetEntityCount()
		{
			return entities.Count;
		}

		private void OnEntityComponentAddedOrRemoved(IEntity entity, int componentTypeId)
		{
			if (groupsForType.ContainsKey(componentTypeId))
			{
				var groups = groupsForType[componentTypeId];

				// TODO: Cache (MemoryPool)
				var groupEvents = new List<GroupChangedEvent>(groups.Count);
				//groupEvents.Clear();

				for (int i = 0; i < groups.Count; ++i)
				{
					groupEvents.Add(groups[i].HandleEntity(entity));
				}

				for (int i = 0; i < groupEvents.Count; ++i)
				{
					if (groupEvents[i] != null)
					{
						groupEvents[i](groups[i], entity);
					}
				}
			}
		}

		private void OnEntityComponentReplaced(IEntity entity, int componentTypeId)
		{
			if (groupsForType.ContainsKey(componentTypeId))
			{
				var groups = groupsForType[componentTypeId];

				for (int i = 0; i < groups.Count; ++i)
				{
					groups[i].UpdateEntity(entity);
				}
			}
		}
	}
}
