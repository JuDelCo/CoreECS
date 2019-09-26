using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public static class ComponentTypeId<T> where T : IComponent
	{
		public static readonly int Value = ComponentTypeId.Get(typeof(T));
	}

	public static class ComponentTypeId
	{
		private static int typeIdGenerator = 0;
		private static Dictionary<Type, int> componentTypeIds = new Dictionary<Type, int>();

		public static int Get(Type type)
		{
			int componentTypeId = -1;

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
