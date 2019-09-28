using System.Collections.Generic;

namespace Ju.ECS
{
	public partial class Matcher : IMatcher
	{
		public IMatcher AllOf<T>() where T : IComponent
		{
			AddTypes(allOfTypes, new List<int>() { ComponentLookup<T>.Id });
			return this;
		}

		public IMatcher AllOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			AddTypes(allOfTypes, new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id });
			return this;
		}

		public IMatcher AllOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			AddTypes(allOfTypes, new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id, ComponentLookup<T3>.Id });
			return this;
		}

		public IMatcher AnyOf<T>() where T : IComponent
		{
			AddTypes(anyOfTypes, new List<int>() { ComponentLookup<T>.Id });
			return this;
		}

		public IMatcher AnyOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			AddTypes(anyOfTypes, new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id });
			return this;
		}

		public IMatcher AnyOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			AddTypes(anyOfTypes, new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id, ComponentLookup<T3>.Id });
			return this;
		}

		public IMatcher NoneOf<T>() where T : IComponent
		{
			AddTypes(noneOfTypes, new List<int>() { ComponentLookup<T>.Id });
			return this;
		}

		public IMatcher NoneOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			AddTypes(noneOfTypes, new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id });
			return this;
		}

		public IMatcher NoneOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			AddTypes(noneOfTypes, new List<int>() { ComponentLookup<T1>.Id, ComponentLookup<T2>.Id, ComponentLookup<T3>.Id });
			return this;
		}
	}
}
