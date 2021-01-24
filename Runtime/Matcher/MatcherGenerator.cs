// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;
using Ju.ECS.Internal;

namespace Ju.ECS
{
	public static class MatcherGenerator
	{
		public static IMatcher AllOf<T>() where T : IComponent
		{
			var types = new List<int>() { ComponentLookup<T>.Id };
			return new Matcher(types, null, null);
		}

		public static IMatcher AllOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			var types = new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id };
			return new Matcher(types, null, null);
		}

		public static IMatcher AllOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			var types = new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id, ComponentLookup<T3>.Id };
			return new Matcher(types, null, null);
		}

		public static IMatcher AllOf<T1, T2, T3, T4>() where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
		{
			var types = new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id, ComponentLookup<T3>.Id, ComponentLookup<T4>.Id };
			return new Matcher(types, null, null);
		}

		public static IMatcher AnyOf<T>() where T : IComponent
		{
			var types = new List<int>() { ComponentLookup<T>.Id };
			return new Matcher(null, types, null);
		}

		public static IMatcher AnyOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			var types = new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id };
			return new Matcher(null, types, null);
		}

		public static IMatcher AnyOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			var types = new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id, ComponentLookup<T3>.Id };
			return new Matcher(null, types, null);
		}

		public static IMatcher AnyOf<T1, T2, T3, T4>() where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
		{
			var types = new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id, ComponentLookup<T3>.Id, ComponentLookup<T4>.Id };
			return new Matcher(null, types, null);
		}
	}
}
