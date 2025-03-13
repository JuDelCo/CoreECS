// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.ECS.Internal
{
	public static class ComponentLookup<T> where T : IComponent
	{
		public static readonly int Id = ComponentType.GetId(typeof(T));
		public static T[] Array => componentArray;
		public static Stack<int> UnusedIndices => unusedIndices;

		private static T[] componentArray = new T[0];
		private static readonly Stack<int> unusedIndices = new Stack<int>(10000);

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

	public static class ComponentTypeIdsPerContext
	{
		private static int[][] array = new int[0][];

		public static void SetContext(int contextId, int componentTypeCount)
		{
			if ((array.Length - 1) < contextId)
			{
				var oldLength = array.Length;
				var newArray = new int[oldLength + 1][];
				System.Array.Copy(array, newArray, oldLength);
				array = newArray;

				array[contextId] = new int[componentTypeCount];

				for (int i = 0; i < componentTypeCount; ++i)
				{
					array[contextId][i] = -1;
				}
			}
		}

		public static int GetTypeId(int contextId, int componentTypeId)
		{
			int index = -1;
			var componentArray = array[contextId];
			int length = componentArray.Length;

			for (int i = 0; i < length; ++i)
			{
				if (componentArray[i] == componentTypeId)
				{
					index = i;
					break;
				}
			}

			if (index == -1)
			{
				for (int i = 0; i < length; ++i)
				{
					if (componentArray[i] == -1)
					{
						componentArray[i] = componentTypeId;
						index = i;
						break;
					}
				}
			}

			return index;
		}
	}

	public static class ComponentType
	{
		private static int typeIdGenerator = 0;
		private static readonly Dictionary<Type, int> componentTypeIds = new Dictionary<Type, int>();

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
