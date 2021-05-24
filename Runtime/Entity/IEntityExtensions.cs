// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.ECS.Internal;

namespace Ju.ECS
{
	public static class IEntityExtensions
	{
		public static IEntity Add<T>(this IEntity entity, T component) where T : IComponent
		{
			var componentTypeId = ComponentLookup<T>.Id;

			if (entity.HasComponent(componentTypeId))
			{
				throw new Exception($"Entity already has a component of type {typeof(T).Name}");
			}

			var componentPoolIndex = ComponentLookup<T>.New();
			ComponentLookup<T>.Array[componentPoolIndex] = component;
			entity.AddComponent(componentTypeId, componentPoolIndex, ComponentLookup<T>.UnusedIndices);

			return entity;
		}

		public static IEntity Replace<T>(this IEntity entity, T component) where T : IComponent
		{
			var componentTypeId = ComponentLookup<T>.Id;

			if (entity.HasComponent(componentTypeId))
			{
				var componentPoolIndex = entity.GetComponentPoolIndex(componentTypeId);
				ComponentLookup<T>.Array[componentPoolIndex] = component;
				entity.ReplaceComponent(componentTypeId);
			}
			else
			{
				entity.Add<T>(component);
			}

			return entity;
		}

		public static IEntity Remove<T>(this IEntity entity) where T : IComponent
		{
			var componentTypeId = ComponentLookup<T>.Id;

			if (!entity.HasComponent(componentTypeId))
			{
				throw new Exception($"Entity doesn't have a component of type {typeof(T).Name}");
			}

			entity.RemoveComponent(componentTypeId);

			return entity;
		}

		public static bool Has<T>(this IEntity entity) where T : IComponent
		{
			return entity.HasComponent(ComponentLookup<T>.Id);
		}

		public static bool Has<T1, T2>(this IEntity entity) where T1 : IComponent where T2 : IComponent
		{
			return entity.HasComponent(ComponentLookup<T1>.Id) &&
					entity.HasComponent(ComponentLookup<T2>.Id);
		}

		public static bool Has<T1, T2, T3>(this IEntity entity) where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			return entity.HasComponent(ComponentLookup<T1>.Id) &&
					entity.HasComponent(ComponentLookup<T2>.Id) &&
					entity.HasComponent(ComponentLookup<T3>.Id);
		}

		public static bool Has<T1, T2, T3, T4>(this IEntity entity) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
		{
			return entity.HasComponent(ComponentLookup<T1>.Id) &&
					entity.HasComponent(ComponentLookup<T2>.Id) &&
					entity.HasComponent(ComponentLookup<T3>.Id) &&
					entity.HasComponent(ComponentLookup<T4>.Id);
		}

		public static T Get<T>(this IEntity entity) where T : IComponent
		{
			var componentTypeId = ComponentLookup<T>.Id;

			if (!entity.HasComponent(componentTypeId))
			{
				throw new Exception($"Entity doesn't have a component of type {typeof(T).Name}");
			}

			return ComponentLookup<T>.Array[entity.GetComponentPoolIndex(componentTypeId)];
		}
	}
}
