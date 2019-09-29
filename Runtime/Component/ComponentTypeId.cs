using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public static class ComponentLookup<T> where T : IComponent
	{
		public static readonly int Id = ComponentType.GetId(typeof(T));
		public static T[] Array => componentArray;
		public static Stack<int> UnusedIndices => unusedIndices;

		private static T[] componentArray = new T[0];
		private static Stack<int> unusedIndices = new Stack<int>(10000);

		public static int New()
		{
			if (unusedIndices.Count == 0)
			{
				IncreasePool(10000);
			}

			return unusedIndices.Pop();
		}

		public static void Remove(int index)
		{
			unusedIndices.Push(index);
		}

		private static void IncreasePool(int amount)
		{
			var oldLength = componentArray.Length;
			var newComponentArray = new T[oldLength + amount];
			System.Array.Copy(componentArray, newComponentArray, oldLength);
			componentArray = newComponentArray;

			for (int i = (componentArray.Length - 1); i >= oldLength; --i)
			{
				unusedIndices.Push(i);
			}
		}
	}

	public static class ComponentType
	{
		private static int typeIdGenerator = 0;
		private static Dictionary<Type, int> componentTypeIds = new Dictionary<Type, int>();

		public static int GetId(Type type)
		{
			int componentTypeId;

			if (componentTypeIds.ContainsKey(type))
			{
				componentTypeId = componentTypeIds[type];
			}
			else
			{
				componentTypeId = typeIdGenerator++;
				componentTypeIds.Add(type, componentTypeId);
			}

			return componentTypeId;
		}
	}
}
