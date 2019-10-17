using System;
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
			return context.GetGroup(MatcherGenerator.AllOf<T>());
		}

		public static IGroup GetGroup<T1, T2>(this IContext context) where T1 : IComponent where T2 : IComponent
		{
			return context.GetGroup(MatcherGenerator.AllOf<T1, T2>());
		}

		public static IGroup GetGroup<T1, T2, T3>(this IContext context) where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			return context.GetGroup(MatcherGenerator.AllOf<T1, T2, T3>());
		}

		public static IGroup GetGroup<T1, T2, T3, T4>(this IContext context) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
		{
			return context.GetGroup(MatcherGenerator.AllOf<T1, T2, T3, T4>());
		}

		public static IEntity Set<T>(this IContext context, T component) where T : IComponent
		{
			if (context.Has<T>())
			{
				throw new Exception("The group have already a single entity with that component");
			}

			return context.CreateEntity().Add(component);
		}

		public static IEntity Replace<T>(this IContext context, T component) where T : IComponent
		{
			var e = context.GetEntity<T>();

			if (e == null)
			{
				e = context.Set<T>(component);
			}
			else
			{
				e.Replace(component);
			}

			return e;
		}

		public static void Remove<T>(this IContext context) where T : IComponent
		{
			context.GetEntity<T>()?.Destroy();
		}

		public static bool Has<T>(this IContext context) where T : IComponent
		{
			return (context.GetEntity<T>() != null);
		}

		public static T Get<T>(this IContext context) where T : IComponent
		{
			var e = context.GetEntity<T>();

			if (e == null)
			{
				throw new Exception("The group doesn't have a single entity with that component");
			}

			return e.Get<T>();
		}

		private static IEntity GetEntity<T>(this IContext context) where T : IComponent
		{
			return context.GetGroup(MatcherGenerator.AllOf<T>()).GetSingleEntity();
		}
	}
}
