using System.Collections.Generic;

namespace Ju.ECS
{
	public class Systems : IInitializeSystem, IExecuteSystem, ICleanupSystem, ITearDownSystem
	{
		private List<IInitializeSystem> initializeSystems;
		private List<IExecuteSystem> executeSystems;
		private List<ICleanupSystem> cleanupSystems;
		private List<ITearDownSystem> tearDownSystems;

		public Systems()
		{
			initializeSystems = new List<IInitializeSystem>();
			executeSystems = new List<IExecuteSystem>();
			cleanupSystems = new List<ICleanupSystem>();
			tearDownSystems = new List<ITearDownSystem>();
		}

		public Systems Add(ISystem system)
		{
			if (system is IInitializeSystem)
			{
				initializeSystems.Add((IInitializeSystem)system);
			}

			if (system is IExecuteSystem)
			{
				executeSystems.Add((IExecuteSystem)system);
			}

			if (system is ICleanupSystem)
			{
				cleanupSystems.Add((ICleanupSystem)system);
			}

			if (system is ITearDownSystem)
			{
				tearDownSystems.Add((ITearDownSystem)system);
			}

			return this;
		}

		public void Initialize()
		{
			for (int i = 0, count = initializeSystems.Count; i < count; ++i)
			{
				initializeSystems[i].Initialize();
			}
		}

		public void Execute()
		{
			for (int i = 0, count = executeSystems.Count; i < count; ++i)
			{
				executeSystems[i].Execute();
			}
		}

		public void Cleanup()
		{
			for (int i = 0, count = cleanupSystems.Count; i < count; ++i)
			{
				cleanupSystems[i].Cleanup();
			}
		}

		public void TearDown()
		{
			for (int i = 0, count = tearDownSystems.Count; i < count; ++i)
			{
				tearDownSystems[i].TearDown();
			}
		}

		public void ActivateReactiveSystems()
		{
			for (int i = 0, count = executeSystems.Count; i < count; ++i)
			{
				var system = executeSystems[i];

				if (system is IReactiveSystem)
				{
					((IReactiveSystem)system).Activate();
				}

				if (system is Systems)
				{
					((Systems)system).ActivateReactiveSystems();
				}
			}
		}

		public void DeactivateReactiveSystems()
		{
			for (int i = 0, count = executeSystems.Count; i < count; ++i)
			{
				var system = executeSystems[i];

				if (system is IReactiveSystem)
				{
					((IReactiveSystem)system).Deactivate();
				}

				if (system is Systems)
				{
					((Systems)system).DeactivateReactiveSystems();
				}
			}
		}

		public void ClearReactiveSystems()
		{
			for (int i = 0, count = executeSystems.Count; i < count; ++i)
			{
				var system = executeSystems[i];

				if (system is IReactiveSystem)
				{
					((IReactiveSystem)system).Clear();
				}

				if (system is Systems)
				{
					((Systems)system).ClearReactiveSystems();
				}
			}
		}
	}
}
