using System;

namespace Ju.ECS
{
	public static class IEntityExtensions
	{
		public static IEntity Add<T>(this IEntity entity, T component) where T : IComponent
		{
			var componentTypeId = ComponentType<T>.Id;

			if (entity.HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity already has a component of type {0}", typeof(T).Name));
			}

			var componentPoolIndex = ComponentType<T>.New();
			ComponentType<T>.componentPool[componentPoolIndex] = component;
			entity.AddComponent(componentTypeId, componentPoolIndex);

			return entity;
		}

		public static IEntity Add<T>(this IEntity entity) where T : IComponent, new()
		{
			var componentTypeId = ComponentType<T>.Id;

			if (entity.HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity already has a component of type {0}", typeof(T).Name));
			}

			var componentPoolIndex = ComponentType<T>.New();
			ComponentType<T>.componentPool[componentPoolIndex] = new T();
			entity.AddComponent(componentTypeId, componentPoolIndex);

			return entity;
		}

		public static IEntity Replace<T>(this IEntity entity, T component) where T : IComponent
		{
			var componentTypeId = ComponentType<T>.Id;

			if (entity.HasComponent(componentTypeId))
			{
				var componentPoolIndex = entity.GetComponentPoolIndex(componentTypeId);
				ComponentType<T>.componentPool[componentPoolIndex] = component;
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
			var componentTypeId = ComponentType<T>.Id;

			if (!entity.HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity does not have a component of type {0}", typeof(T).Name));
			}

			var componentPoolIndex = entity.GetComponentPoolIndex(componentTypeId);
			ComponentType<T>.Remove(componentPoolIndex);
			entity.RemoveComponent(ComponentType<T>.Id);

			return entity;
		}

		public static bool Has<T>(this IEntity entity) where T : IComponent
		{
			return entity.HasComponent(ComponentType<T>.Id);
		}

		public static bool Has(this IEntity entity, int componentTypeId)
		{
			return entity.HasComponent(componentTypeId);
		}

		public static T Get<T>(this IEntity entity) where T : IComponent
		{
			var componentTypeId = ComponentType<T>.Id;

			if (!entity.HasComponent(componentTypeId))
			{
				throw new Exception(string.Format("Entity does not have a component of type {0}", typeof(T).Name));
			}

			return ComponentType<T>.componentPool[entity.GetComponentPoolIndex(componentTypeId)];
		}
	}
}
