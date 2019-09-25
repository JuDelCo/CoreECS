using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Context : IContext
	{
		public event ContextEntityEvent OnEntityCreated = delegate { };
		public event ContextEntityEvent OnEntityDestroyed = delegate { };
		public event ContextGroupEvent OnGroupCreated = delegate { };

		private List<IEntity> entities;
		private Dictionary<Type, List<IGroup>> groupsForType;
		private Dictionary<IMatcher, IGroup> groupCache;

		public Context() : this(100000)
		{
		}

		public Context(int capacity)
		{
			entities = new List<IEntity>(capacity);
			groupsForType = new Dictionary<Type, List<IGroup>>(100);
			groupCache = new Dictionary<IMatcher, IGroup>();
		}

		public IEntity CreateEntity()
		{
			// TODO: Cache (Reuse destroyed entities)
			var entity = new Entity();

			entities.Add(entity);

			entity.OnComponentAdded += OnEntityComponentAddedOrRemoved;
			entity.OnComponentReplaced += OnEntityComponentReplaced;
			entity.OnComponentRemoved += OnEntityComponentAddedOrRemoved;

			OnEntityCreated(this, entity);

			return entity;
		}

		public bool HasEntity(IEntity entity)
		{
			return entities.Contains(entity);
		}

		public void DestroyEntity(IEntity entity)
		{
			if (!entities.Remove(entity))
			{
				throw new Exception("Context does not contain the entity");
			}

			// TODO: Handle this in Entity class
			entity.OnComponentAdded -= OnEntityComponentAddedOrRemoved;
			entity.OnComponentReplaced -= OnEntityComponentReplaced;
			entity.OnComponentRemoved -= OnEntityComponentAddedOrRemoved;

			OnEntityDestroyed(this, entity);
		}

		public void DestroyAllEntities()
		{
			for (int i = entities.Count; i > 0; --i)
			{
				DestroyEntity(entities[i]);
			}

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

			foreach (var type in matcher.GetTypes())
			{
				if (!groupsForType.ContainsKey(type))
				{
					groupsForType.Add(type, new List<IGroup>());
				}

				groupsForType[type].Add(group);
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
			var type = component.GetType();

			if (groupsForType.ContainsKey(type))
			{
				var groups = groupsForType[type];

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
			var type = newComponent.GetType();

			if (groupsForType.ContainsKey(type))
			{
				var groups = groupsForType[type];

				for (int i = 0; i < groups.Count; ++i)
				{
					groups[i].UpdateEntity(entity, previousComponent, newComponent);
				}
			}
		}
	}
}
