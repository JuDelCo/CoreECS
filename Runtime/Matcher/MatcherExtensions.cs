using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public partial class Matcher : IMatcher
	{
		public IMatcher AllOf<T>() where T : IComponent
		{
			AddTypes(allOfTypes, new List<Type>() { typeof(T) });
			return this;
		}

		public IMatcher AllOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			AddTypes(allOfTypes, new List<Type>() { typeof(T1), typeof(T2) });
			return this;
		}

		public IMatcher AllOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			AddTypes(allOfTypes, new List<Type>() { typeof(T1), typeof(T2), typeof(T3) });
			return this;
		}

		public IMatcher AnyOf<T>() where T : IComponent
		{
			AddTypes(anyOfTypes, new List<Type>() { typeof(T) });
			return this;
		}

		public IMatcher AnyOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			AddTypes(anyOfTypes, new List<Type>() { typeof(T1), typeof(T2) });
			return this;
		}

		public IMatcher AnyOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			AddTypes(anyOfTypes, new List<Type>() { typeof(T1), typeof(T2), typeof(T3) });
			return this;
		}

		public IMatcher NoneOf<T>() where T : IComponent
		{
			AddTypes(noneOfTypes, new List<Type>() { typeof(T) });
			return this;
		}

		public IMatcher NoneOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			AddTypes(noneOfTypes, new List<Type>() { typeof(T1), typeof(T2) });
			return this;
		}

		public IMatcher NoneOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			AddTypes(noneOfTypes, new List<Type>() { typeof(T1), typeof(T2), typeof(T3) });
			return this;
		}
	}
}
