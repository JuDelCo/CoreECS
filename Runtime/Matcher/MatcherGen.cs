using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public static class MatcherGen
	{
		public static IMatcher AllOf(List<Type> types)
		{
			return new Matcher(types, null, null);
		}

		public static IMatcher AnyOf(List<Type> types)
		{
			return new Matcher(null, types, null);
		}

		public static IMatcher AllOf<T>() where T : IComponent
		{
			var types = new List<Type>() { typeof(T) };
			return new Matcher(types, null, null);
		}

		public static IMatcher AllOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			var types = new List<Type>() { typeof(T1), typeof(T2) };
			return new Matcher(types, null, null);
		}

		public static IMatcher AllOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			var types = new List<Type>() { typeof(T1), typeof(T2), typeof(T3) };
			return new Matcher(types, null, null);
		}

		public static IMatcher AnyOf<T>() where T : IComponent
		{
			var types = new List<Type>() { typeof(T) };
			return new Matcher(null, types, null);
		}

		public static IMatcher AnyOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			var types = new List<Type>() { typeof(T1), typeof(T2) };
			return new Matcher(null, types, null);
		}

		public static IMatcher AnyOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			var types = new List<Type>() { typeof(T1), typeof(T2), typeof(T3) };
			return new Matcher(null, types, null);
		}
	}
}
