using System.Collections.Generic;

namespace Ju.ECS
{
	public abstract class ReactiveSystem : IReactiveSystem
	{
		private readonly ICollector collector;
		private readonly List<IEntity> entities;

		public ReactiveSystem(IContext context)
		{
			entities = new List<IEntity>();
			collector = GetTrigger(context);
		}

		~ReactiveSystem()
		{
			Deactivate();
		}

		protected abstract ICollector GetTrigger(IContext context);
		protected abstract bool Filter(IEntity entity);
		protected abstract void Execute(List<IEntity> entities);

		public void Activate()
		{
			collector.Activate();
		}

		public void Deactivate()
		{
			collector.Deactivate();
		}

		public void Clear()
		{
			collector.ClearCollectedEntities();
		}

		public void Execute()
		{
			if (collector.GetCount() != 0)
			{
				var collectedEntities = collector.GetCollectedEntities();

				for (int i = (collectedEntities.Count - 1); i >= 0; --i)
				{
					if (Filter(collectedEntities[i]))
					{
						entities.Add(collectedEntities[i]);
						collectedEntities[i].Retain();
					}
				}

				collector.ClearCollectedEntities();

				if (entities.Count != 0)
				{
					try
					{
						Execute(entities);
					}
					finally
					{
						for (int i = (entities.Count - 1); i >= 0; --i)
						{
							entities[i].Release();
						}

						entities.Clear();
					}
				}
			}
		}
	}
}
