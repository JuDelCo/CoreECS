// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2021 Juan Delgado (@JuDelCo)

namespace Ju.ECS
{
	public interface ISystem
	{
	}

	public interface IInitializeSystem : ISystem
	{
		void Initialize();
	}

	public interface IExecuteSystem : ISystem
	{
		void Execute();
	}

	public interface IReactiveSystem : IExecuteSystem
	{
		void Activate();
		void Deactivate();
		void Clear();
	}

	public interface ICleanupSystem : ISystem
	{
		void Cleanup();
	}

	public interface ITearDownSystem : ISystem
	{
		void TearDown();
	}
}
