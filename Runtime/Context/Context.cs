using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Context : IContext
	{
		public event ContextEntityEvent OnEntityCreated = delegate { };
		public event ContextEntityEvent OnEntityDestroyed = delegate { };
		public event ContextGroupEvent OnGroupCreated = delegate { };

		private HashSet<IEntity> entitiesHashSet;
		private List<IEntity> entities;
		private Dictionary<int, List<IGroup>> groupsForType;
		private Dictionary<IMatcher, IGroup> groupCache;

		public Context() : this(100000)
		{
		}

		public Context(int capacity)
		{
			entitiesHashSet = new HashSet<IEntity>(new EntityEqualityComparer());
			entities = new List<IEntity>(capacity);
			groupsForType = new Dictionary<int, List<IGroup>>(100);
			groupCache = new Dictionary<IMatcher, IGroup>();
		}

		public IEntity CreateEntity()
		{
			// TODO: Cache (Reuse destroyed entities)
			var entity = new Entity();

			entitiesHashSet.Add(entity);
			entities.Add(entity);

			entity.OnComponentAdded += OnEntityComponentAddedOrRemoved;
			entity.OnComponentReplaced += OnEntityComponentReplaced;
			entity.OnComponentRemoved += OnEntityComponentAddedOrRemoved;

			OnEntityCreated(this, entity);

			return entity;
		}

		public bool HasEntity(IEntity entity)
		{
			return entitiesHashSet.Contains(entity);
		}

		public void DestroyEntity(IEntity entity)
		{
			if (!entitiesHashSet.Remove(entity))
			{
				throw new Exception("Context does not contain the entity");
			}

			entities.Remove(entity);

			// TODO: Handle this in Entity class
			entity.OnComponentAdded -= OnEntityComponentAddedOrRemoved;
			entity.OnComponentReplaced -= OnEntityComponentReplaced;
			entity.OnComponentRemoved -= OnEntityComponentAddedOrRemoved;

			// TODO: Re-enable
			//OnEntityDestroyed(this, entity);
		}

		public void DestroyAllEntities()
		{
			for (int i = entities.Count; i > 0; --i)
			{
				DestroyEntity(entities[i]);
			}

			entitiesHashSet.Clear();
			entities.Clear();
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

			foreach (var componentTypeId in matcher.GetTypes())
			{
				if (!groupsForType.ContainsKey(componentTypeId))
				{
					groupsForType.Add(componentTypeId, new List<IGroup>());
				}

				groupsForType[componentTypeId].Add(group);
			}

			OnGroupCreated(this, group);

			return group;
		}

		public int GetEntityCount()
		{
			return entities.Count;
		}

		private void OnEntityComponentAddedOrRemoved(IEntity entity, IComponent component)
		{
			var componentTypeId = component.GetTypeId();

			if (groupsForType.ContainsKey(componentTypeId))
			{
				var groups = groupsForType[componentTypeId];

				// TODO: Cache (MemoryPool)
				var groupEvents = new List<GroupChangedEvent>(groups.Count);

				for (int i = 0; i < groups.Count; ++i)
				{
					groupEvents.Add(groups[i].HandleEntity(entity));
				}

				for (int i = 0; i < groupEvents.Count; ++i)
				{
					if (groupEvents[i] != null)
					{
						groupEvents[i](groups[i], entity, component);
					}
				}
			}
		}

		private void OnEntityComponentReplaced(IEntity entity, IComponent previousComponent, IComponent newComponent)
		{
			var componentTypeId = newComponent.GetTypeId();

			if (groupsForType.ContainsKey(componentTypeId))
			{
				var groups = groupsForType[componentTypeId];

				for (int i = 0; i < groups.Count; ++i)
				{
					groups[i].UpdateEntity(entity, previousComponent, newComponent);
				}
			}
		}
	}
}
