using System.Collections.Generic;

namespace Ju.ECS
{
	public abstract class ReactiveSystem : IReactiveSystem
	{
		private ICollector collector;
		private List<IEntity> entities;

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
				foreach (var e in collector.GetCollectedEntities())
				{
					if (Filter(e))
					{
						entities.Add(e);
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
						entities.Clear();
					}
				}
			}
		}
	}
}
