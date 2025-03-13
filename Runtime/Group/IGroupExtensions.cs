// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2025 Juan Delgado (@JuDelCo)

namespace Ju.ECS
{
	public static class IGroupExtensions
	{
		public static ICollector CreateCollector(this IGroup group, GroupEvent groupEvent = GroupEvent.Added)
		{
			return new Collector(group, groupEvent);
		}
	}
}
