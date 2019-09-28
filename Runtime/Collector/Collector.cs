using System.Collections.Generic;

namespace Ju.ECS
{
	public class Collector : ICollector
	{
		private HashSet<uint> collectedEntitiesHashSet;
		private List<IEntity> collectedEntities;
		private IGroup group;
		private GroupEvent groupEvent;

		private Collector()
		{
			collectedEntitiesHashSet = new HashSet<uint>();
			collectedEntities = new List<IEntity>(1000);
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
					group.OnEntityAdded -= OnEntityGroupEvent;
					group.OnEntityAdded += OnEntityGroupEvent;
					break;
				case GroupEvent.Removed:
					group.OnEntityRemoved -= OnEntityGroupEvent;
					group.OnEntityRemoved += OnEntityGroupEvent;
					break;
				case GroupEvent.AddedOrRemoved:
					group.OnEntityAdded -= OnEntityGroupEvent;
					group.OnEntityAdded += OnEntityGroupEvent;
					group.OnEntityRemoved -= OnEntityGroupEvent;
					group.OnEntityRemoved += OnEntityGroupEvent;
					break;
			}
		}

		public void Deactivate()
		{
			group.OnEntityAdded -= OnEntityGroupEvent;
			group.OnEntityRemoved -= OnEntityGroupEvent;

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
