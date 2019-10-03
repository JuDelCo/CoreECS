using System.Collections.Generic;

namespace Ju.ECS
{
	public static class IContextExtensions
	{
		public static List<IEntity> GetEntities(this IContext context, IMatcher matcher)
		{
			return context.GetGroup(matcher).GetEntities();
		}

		public static IGroup GetGroup<T>(this IContext context) where T : IComponent
		{
			return context.GetGroup(MatcherGen.AllOf<T>());
		}

		public static IGroup GetGroup<T1, T2>(this IContext context) where T1 : IComponent where T2 : IComponent
		{
			return context.GetGroup(MatcherGen.AllOf<T1, T2>());
		}

		public static IGroup GetGroup<T1, T2, T3>(this IContext context) where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			return context.GetGroup(MatcherGen.AllOf<T1, T2, T3>());
		}

		public static IGroup GetGroup<T1, T2, T3, T4>(this IContext context) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
		{
			return context.GetGroup(MatcherGen.AllOf<T1, T2, T3>());
		}
	}
}
