// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2025 Juan Delgado (@JuDelCo)

using System.Collections.Generic;

namespace Ju.ECS
{
	public enum GroupEvent : byte
	{
		Added,
		Removed,
		AddedOrRemoved
	}

	public interface ICollector
	{
		void Activate();
		void Deactivate();

		int GetCount();
		List<IEntity> GetCollectedEntities();
		void ClearCollectedEntities();
	}
}
