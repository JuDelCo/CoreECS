// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2021 Juan Delgado (@JuDelCo)

namespace Ju.ECS
{
	public static class ICollectorExtensions
	{
		public static ICollector CreateCollector(this IContext context, IMatcher matcher, GroupEvent groupEvent = GroupEvent.Added)
		{
			return new Collector(context.GetGroup(matcher), groupEvent);
		}
	}
}
