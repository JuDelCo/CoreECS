using System.Collections.Generic;

namespace Ju.ECS
{
	public partial class Matcher : IMatcher
	{
		private List<int> allOfTypes;
		private List<int> anyOfTypes;
		private List<int> noneOfTypes;
		private List<int> allTypes;

		private bool isHashCached = false;
		private int cachedHash = 0;

		private Matcher()
		{
			allOfTypes = new List<int>();
			anyOfTypes = new List<int>();
			noneOfTypes = new List<int>();
			allTypes = new List<int>();
		}

		public Matcher(List<int> allOf, List<int> anyOf, List<int> noneOf) : this()
		{
			AddTypes(allOfTypes, allOf);
			AddTypes(anyOfTypes, anyOf);
			AddTypes(noneOfTypes, noneOf);
		}

		public bool Matches(IEntity entity)
		{
			bool result = true;

			for (int i = (allOfTypes.Count - 1); i >= 0; --i)
			{
				if (!entity.HasComponent(allOfTypes[i]))
				{
					result = false;
					break;
				}
			}

			if (anyOfTypes.Count > 0)
			{
				var found = false;

				for (int i = (anyOfTypes.Count - 1); i >= 0; --i)
				{
					if (entity.HasComponent(anyOfTypes[i]))
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

			for (int i = (noneOfTypes.Count - 1); i >= 0; --i)
			{
				if (entity.HasComponent(noneOfTypes[i]))
				{
					result = false;
					break;
				}
			}

			return result;
		}

		public IMatcher AllOf(List<int> componentTypeIds)
		{
			AddTypes(allOfTypes, componentTypeIds);
			return this;
		}

		public IMatcher AnyOf(List<int> componentTypeIds)
		{
			AddTypes(anyOfTypes, componentTypeIds);
			return this;
		}

		public IMatcher NoneOf(List<int> componentTypeIds)
		{
			AddTypes(noneOfTypes, componentTypeIds);
			return this;
		}

		public List<int> GetTypes()
		{
			return allTypes;
		}

		private void AddTypes(List<int> target, List<int> types)
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

		private void AddType(int type)
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
					for (int i = (allTypes.Count - 1); i >= 0; --i)
					{
						cachedHash += GetDeterministicHashCode(allTypes[i].GetType().Name);
					}
				}

				isHashCached = true;
			}

			return cachedHash;
		}

		private int GetDeterministicHashCode(string typeName)
		{
			unchecked
			{
				int hash1 = (5381 << 16) + 5381;
				int hash2 = hash1;

				for (int i = 0; i < typeName.Length; i += 2)
				{
					hash1 = ((hash1 << 5) + hash1) ^ typeName[i];

					if (i == typeName.Length - 1)
					{
						break;
					}

					hash2 = ((hash2 << 5) + hash2) ^ typeName[i + 1];
				}

				return hash1 + (hash2 * 1566083941);
			}
		}
	}
}
