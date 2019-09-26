using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Group : IGroup
	{
		public event GroupChangedEvent OnEntityAdded = delegate { };
		public event GroupUpdatedEvent OnEntityUpdated = delegate { };
		public event GroupChangedEvent OnEntityRemoved = delegate { };

		private IMatcher matcher;
		private HashSet<IEntity> entitiesHashSet;
		private List<IEntity> entities;

		public Group(IMatcher matcher)
		{
			entitiesHashSet = new HashSet<IEntity>(new EntityEqualityComparer());
			entities = new List<IEntity>();
			this.matcher = matcher;
		}

		public int GetCount()
		{
			return entitiesHashSet.Count;
		}

		public bool ContainsEntity(IEntity entity)
		{
			return entitiesHashSet.Contains(entity);
		}

		public List<IEntity> GetEntities()
		{
			return entities;
		}

		public IEntity GetSingleEntity()
		{
			if (entities.Count != 1)
			{
				throw new Exception(string.Format("The group does not have a single entity (count: {0})", entities.Count));
			}

			return entities[0];
		}

		public IMatcher GetMatcher()
		{
			return matcher;
		}

		public void HandleEntitySilently(IEntity entity)
		{
			if (matcher.Matches(entity))
			{
				if (!entitiesHashSet.Contains(entity))
				{
					entitiesHashSet.Add(entity);
					entities.Add(entity);
				}
			}
			else
			{
				if (entitiesHashSet.Contains(entity))
				{
					entitiesHashSet.Remove(entity);
					entities.Remove(entity);
				}
			}
		}

		public GroupChangedEvent HandleEntity(IEntity entity)
		{
			GroupChangedEvent groupEvent = null;

			if (matcher.Matches(entity))
			{
				if (!entitiesHashSet.Contains(entity))
				{
					entitiesHashSet.Add(entity);
					entities.Add(entity);
					groupEvent = OnEntityAdded;
				}
			}
			else
			{
				if (entitiesHashSet.Contains(entity))
				{
					entitiesHashSet.Remove(entity);
					entities.Remove(entity);
					groupEvent = OnEntityRemoved;
				}
			}

			return groupEvent;
		}

		public void UpdateEntity(IEntity entity, IComponent previousComponent, IComponent newComponent)
		{
			if (entitiesHashSet.Contains(entity))
			{
				OnEntityRemoved(this, entity, previousComponent);
				OnEntityAdded(this, entity, newComponent);
				OnEntityUpdated(this, entity, previousComponent, newComponent);
			}
		}
	}
}
