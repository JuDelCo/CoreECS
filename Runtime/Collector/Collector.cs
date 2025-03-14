// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public class Collector : ICollector
	{
		private bool[] collectedEntitiesCheck;
		private readonly List<IEntity> collectedEntities;
		private readonly IGroup group;
		private readonly GroupEvent groupEvent;
		private readonly GroupChangedEvent onEntityGroupEventCache;

		private Collector()
		{
			collectedEntitiesCheck = new bool[0];
			collectedEntities = new List<IEntity>(1000);
			onEntityGroupEventCache = OnEntityGroupEvent;
		}

		public Collector(IGroup group, GroupEvent groupEvent) : this()
		{
			this.group = group;
			this.groupEvent = groupEvent;

			Activate();
		}

		~Collector()
		{
			Deactivate();
		}

		public void Activate()
		{
			switch (groupEvent)
			{
				case GroupEvent.Added:
					group.OnEntityAdded += onEntityGroupEventCache;
					break;
				case GroupEvent.Removed:
					group.OnEntityRemoved += onEntityGroupEventCache;
					break;
				case GroupEvent.AddedOrRemoved:
					group.OnEntityAdded += onEntityGroupEventCache;
					group.OnEntityRemoved += onEntityGroupEventCache;
					break;
			}
		}

		public void Deactivate()
		{
			group.OnEntityAdded -= onEntityGroupEventCache;
			group.OnEntityRemoved -= onEntityGroupEventCache;

			ClearCollectedEntities();
		}

		public int GetCount()
		{
			return collectedEntities.Count;
		}

		public List<IEntity> GetCollectedEntities()
		{
			return collectedEntities;
		}

		public void ClearCollectedEntities()
		{
			for (int i = (collectedEntities.Count - 1); i >= 0; --i)
			{
				collectedEntitiesCheck[collectedEntities[i].GetUuid()] = false;
				collectedEntities[i].Release();
			}

			collectedEntities.Clear();
		}

		private void OnEntityGroupEvent(IGroup group, IEntity entity)
		{
			if (entity.GetUuid() >= collectedEntitiesCheck.Length)
			{
				// Increase Check Array
				{
					int target = (int)entity.GetUuid();
					int inc = 10000;
					target = (target / inc) * inc + ((target % inc) > (inc / 2) ? (inc * 2) : inc);

					var newCollectedEntitiesCheck = new bool[target];
					Array.Copy(collectedEntitiesCheck, newCollectedEntitiesCheck, collectedEntitiesCheck.Length);
					collectedEntitiesCheck = newCollectedEntitiesCheck;
				}
			}

			if (!collectedEntitiesCheck[entity.GetUuid()])
			{
				collectedEntitiesCheck[entity.GetUuid()] = true;
				collectedEntities.Add(entity);
				entity.Retain();
			}
		}
	}
}
