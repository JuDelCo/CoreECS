using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Group : IGroup
	{
		public event GroupChangedEvent OnEntityAdded;
		public event GroupChangedEvent OnEntityRemoved;

		private readonly IMatcher matcher;
		private readonly List<IEntity> entities;
		private bool[] entitiesCheck;

		public Group(IMatcher matcher)
		{
			entitiesCheck = new bool[0];
			entities = new List<IEntity>();
			this.matcher = matcher;
		}

		public int GetCount()
		{
			return entities.Count;
		}

		public List<IEntity> GetEntities()
		{
			return entities;
		}

		public IEntity GetSingleEntity()
		{
			IEntity entity = null;

			if (entities.Count > 1)
			{
				throw new Exception(string.Format("The group doesn't have a single entity (count: {0})", entities.Count));
			}
			else if (entities.Count == 1)
			{
				entity = entities[0];
			}

			return entity;
		}

		public IMatcher GetMatcher()
		{
			return matcher;
		}

		public void HandleEntitySilently(IEntity entity)
		{
			if (entity.GetUuid() >= entitiesCheck.Length)
			{
				IncreaseCheckArray((int)entity.GetUuid());
			}

			if (matcher.Matches(entity))
			{
				if (!entitiesCheck[entity.GetUuid()])
				{
					entitiesCheck[entity.GetUuid()] = true;
					entities.Add(entity);
					entity.Retain();
				}
			}
			else
			{
				if (entitiesCheck[entity.GetUuid()])
				{
					entitiesCheck[entity.GetUuid()] = false;
					entities.Remove(entity);
					entity.Release();
				}
			}
		}

		public GroupChangedEvent HandleEntity(IEntity entity)
		{
			GroupChangedEvent groupEvent = null;

			if (entity.GetUuid() >= entitiesCheck.Length)
			{
				IncreaseCheckArray((int)entity.GetUuid());
			}

			if (matcher.Matches(entity))
			{
				if (!entitiesCheck[entity.GetUuid()])
				{
					entitiesCheck[entity.GetUuid()] = true;
					entities.Add(entity);
					entity.Retain();
					groupEvent = OnEntityAdded;
				}
			}
			else
			{
				if (entitiesCheck[entity.GetUuid()])
				{
					entitiesCheck[entity.GetUuid()] = false;
					entities.Remove(entity);
					entity.Release();
					groupEvent = OnEntityRemoved;
				}
			}

			return groupEvent;
		}

		public void UpdateEntity(IEntity entity)
		{
			if (entity.GetUuid() >= entitiesCheck.Length)
			{
				IncreaseCheckArray((int)entity.GetUuid());
			}

			if (entitiesCheck[entity.GetUuid()])
			{
				if (OnEntityRemoved != null)
				{
					OnEntityRemoved(this, entity);
				}

				if (OnEntityAdded != null)
				{
					OnEntityAdded(this, entity);
				}
			}
		}

		private void IncreaseCheckArray(int target)
		{
			int inc = 10000;
			target = (target / inc) * inc + ((target % inc) > (inc / 2) ? (inc * 2) : inc);

			var newEntitiesCheck = new bool[target];
			Array.Copy(entitiesCheck, newEntitiesCheck, entitiesCheck.Length);
			entitiesCheck = newEntitiesCheck;
		}
	}
}
