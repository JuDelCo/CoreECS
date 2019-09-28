using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Group : IGroup
	{
		public event GroupChangedEvent OnEntityAdded = delegate { };
		public event GroupChangedEvent OnEntityRemoved = delegate { };

		private IMatcher matcher;
		private HashSet<uint> entitiesHashSet;
		private List<IEntity> entities;

		public Group(IMatcher matcher)
		{
			entitiesHashSet = new HashSet<uint>();
			entities = new List<IEntity>();
			this.matcher = matcher;
		}

		public int GetCount()
		{
			return entitiesHashSet.Count;
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
				if (!entitiesHashSet.Contains(entity.GetUuid()))
				{
					entitiesHashSet.Add(entity.GetUuid());
					entities.Add(entity);
				}
			}
			else
			{
				if (entitiesHashSet.Contains(entity.GetUuid()))
				{
					entitiesHashSet.Remove(entity.GetUuid());
					entities.Remove(entity);
				}
			}
		}

		public GroupChangedEvent HandleEntity(IEntity entity)
		{
			GroupChangedEvent groupEvent = null;

			if (matcher.Matches(entity))
			{
				if (!entitiesHashSet.Contains(entity.GetUuid()))
				{
					entitiesHashSet.Add(entity.GetUuid());
					entities.Add(entity);
					groupEvent = OnEntityAdded;
				}
			}
			else
			{
				if (entitiesHashSet.Contains(entity.GetUuid()))
				{
					entitiesHashSet.Remove(entity.GetUuid());
					entities.Remove(entity);
					groupEvent = OnEntityRemoved;
				}
			}

			return groupEvent;
		}

		public void UpdateEntity(IEntity entity)
		{
			if (entitiesHashSet.Contains(entity.GetUuid()))
			{
				OnEntityRemoved(this, entity);
				OnEntityAdded(this, entity);
			}
		}
	}
}
