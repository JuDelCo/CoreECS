using System;

namespace Ju.ECS
{
	public static class IEntityExtensions
	{
		public static IEntity Add<T>(this IEntity entity, T component) where T : IComponent
		{
			var componentTypeId = ComponentLookup<T>.Id;

			if (entity.HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity already has a component of type {0}", typeof(T).Name));
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
				throw new Exception(string.Format("Entity does not have a component of type {0}", typeof(T).Name));
			}

			entity.RemoveComponent(componentTypeId);

			return entity;
		}

		public static bool Has<T>(this IEntity entity) where T : IComponent
		{
			return entity.HasComponent(ComponentLookup<T>.Id);
		}

		public static bool Has(this IEntity entity, int componentTypeId)
		{
			return entity.HasComponent(componentTypeId);
		}

		public static T Get<T>(this IEntity entity) where T : IComponent
		{
			var componentTypeId = ComponentLookup<T>.Id;

			if (!entity.HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity does not have a component of type {0}", typeof(T).Name));
			}

			return ComponentLookup<T>.Array[entity.GetComponentPoolIndex(componentTypeId)];
		}
	}
}
