using System.Collections.Generic;

namespace Ju.ECS
{
	public class Collector : ICollector
	{
		private HashSet<uint> collectedEntitiesHashSet;
		private List<IEntity> collectedEntities;
		private IGroup group;
		private GroupEvent groupEvent;
		private GroupChangedEvent onEntityGroupEventCache;

		private Collector()
		{
			collectedEntitiesHashSet = new HashSet<uint>();
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
			return collectedEntitiesHashSet.Count;
		}

		public List<IEntity> GetCollectedEntities()
		{
			return collectedEntities;
		}

		public void ClearCollectedEntities()
		{
			collectedEntitiesHashSet.Clear();
			collectedEntities.Clear();
		}

		private void OnEntityGroupEvent(IGroup group, IEntity entity)
		{
			if (!collectedEntitiesHashSet.Contains(entity.GetUuid()))
			{
				collectedEntitiesHashSet.Add(entity.GetUuid());
				collectedEntities.Add(entity);
			}
		}
	}
}
