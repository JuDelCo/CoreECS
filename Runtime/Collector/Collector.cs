using System.Collections.Generic;

namespace Ju.ECS
{
	public class Collector : ICollector
	{
		private List<IEntity> collectedEntities;
		private IGroup group;
		private GroupEvent groupEvent;

		private Collector()
		{
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
			return collectedEntities.Count;
		}

		public List<IEntity> GetCollectedEntities()
		{
			return collectedEntities;
		}

		public void ClearCollectedEntities()
		{
			collectedEntities.Clear();
		}

		private void OnEntityGroupEvent(IGroup group, IEntity entity, IComponent component)
		{
			if (!collectedEntities.Contains(entity))
			{
				collectedEntities.Add(entity);
			}
		}
	}
}
