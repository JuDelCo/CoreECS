using System;
using System.Collections.Generic;
using Ju.ECS.Util;

namespace Ju.ECS
{
	public partial class Matcher : IMatcher
	{
		private List<Type> allOfTypes;
		private List<Type> anyOfTypes;
		private List<Type> noneOfTypes;
		private List<Type> allTypes;

		private bool isHashCached = false;
		private int cachedHash = 0;

		private Matcher()
		{
			allOfTypes = new List<Type>();
			anyOfTypes = new List<Type>();
			noneOfTypes = new List<Type>();
			allTypes = new List<Type>();
		}

		public Matcher(List<Type> allOf, List<Type> anyOf, List<Type> noneOf) : this()
		{
			AddTypes(allOfTypes, allOf);
			AddTypes(anyOfTypes, anyOf);
			AddTypes(noneOfTypes, noneOf);
		}

		public bool Matches(IEntity entity)
		{
			bool result = true;

			foreach (var type in allOfTypes)
			{
				if (!entity.HasComponent(type))
				{
					result = false;
					break;
				}
			}

			if (anyOfTypes.Count > 0)
			{
				var found = false;

				foreach (var type in anyOfTypes)
				{
					if (entity.HasComponent(type))
					{
						found = true;
						break;
					}
				}

				if (found == false)
				{
					result = false;
				}
			}

			foreach (var type in noneOfTypes)
			{
				if (entity.HasComponent(type))
				{
					result = false;
					break;
				}
			}

			return result;
		}

		public IMatcher AllOf(List<Type> types)
		{
			AddTypes(allOfTypes, types);
			return this;
		}

		public IMatcher AnyOf(List<Type> types)
		{
			AddTypes(anyOfTypes, types);
			return this;
		}

		public IMatcher NoneOf(List<Type> types)
		{
			AddTypes(noneOfTypes, types);
			return this;
		}

		public List<Type> GetTypes()
		{
			return allTypes;
		}

		private void AddTypes(List<Type> target, List<Type> types)
		{
			if (types != null)
			{
				for (int i = 0; i < types.Count; ++i)
				{
					if (!target.Contains(types[i]))
					{
						target.Add(types[i]);
						AddType(types[i]);
					}
				}
			}
		}

		private void AddType(Type type)
		{
			if (!allTypes.Contains(type))
			{
				allTypes.Add(type);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType() || obj.GetHashCode() != GetHashCode())
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			if (!isHashCached)
			{
				unchecked
				{
					foreach (var type in allTypes)
					{
						cachedHash += type.GetType().Name.GetDeterministicHashCode();
					}
				}

				isHashCached = true;
			}

			return cachedHash;
		}
	}
}
