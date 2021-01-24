// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;

namespace Ju.ECS
{
	public class Systems : IInitializeSystem, IExecuteSystem, ICleanupSystem, ITearDownSystem
	{
		private readonly List<IInitializeSystem> initializeSystems;
		private readonly List<IExecuteSystem> executeSystems;
		private readonly List<ICleanupSystem> cleanupSystems;
		private readonly List<ITearDownSystem> tearDownSystems;

		public Systems()
		{
			initializeSystems = new List<IInitializeSystem>();
			executeSystems = new List<IExecuteSystem>();
			cleanupSystems = new List<ICleanupSystem>();
			tearDownSystems = new List<ITearDownSystem>();
		}

		public Systems Add(ISystem system)
		{
			if (system is IInitializeSystem initializeSystem)
			{
				initializeSystems.Add(initializeSystem);
			}

			if (system is IExecuteSystem executeSystem)
			{
				executeSystems.Add(executeSystem);
			}

			if (system is ICleanupSystem cleanupSystem)
			{
				cleanupSystems.Add(cleanupSystem);
			}

			if (system is ITearDownSystem tearDownSystem)
			{
				tearDownSystems.Add(tearDownSystem);
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

				if (system is IReactiveSystem reactiveSystem)
				{
					reactiveSystem.Activate();
				}

				if (system is Systems systems)
				{
					systems.ActivateReactiveSystems();
				}
			}
		}

		public void DeactivateReactiveSystems()
		{
			for (int i = 0, count = executeSystems.Count; i < count; ++i)
			{
				var system = executeSystems[i];

				if (system is IReactiveSystem reactiveSystem)
				{
					reactiveSystem.Deactivate();
				}

				if (system is Systems systems)
				{
					systems.DeactivateReactiveSystems();
				}
			}
		}

		public void ClearReactiveSystems()
		{
			for (int i = 0, count = executeSystems.Count; i < count; ++i)
			{
				var system = executeSystems[i];

				if (system is IReactiveSystem reactiveSystem)
				{
					reactiveSystem.Clear();
				}

				if (system is Systems systems)
				{
					systems.ClearReactiveSystems();
				}
			}
		}
	}
}
