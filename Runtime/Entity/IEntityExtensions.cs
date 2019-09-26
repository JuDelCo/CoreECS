
namespace Ju.ECS
{
	public static class IEntityExtensions
	{
		public static IEntity AddComponent<T>(this IEntity entity, T component) where T : IComponent
		{
			return entity.AddComponent(component);
		}

		public static IEntity AddComponent<T>(this IEntity entity) where T : IComponent, new()
		{
			return entity.AddComponent(entity.CreateComponent<T>());
		}

		public static IEntity Add<T>(this IEntity entity, T component) where T : IComponent
		{
			return entity.AddComponent(component);
		}

		public static IEntity Add<T>(this IEntity entity) where T : IComponent, new()
		{
			return entity.AddComponent(entity.CreateComponent<T>());
		}

		public static IEntity ReplaceComponent<T>(this IEntity entity, T component) where T : IComponent
		{
			return entity.ReplaceComponent(component);
		}

		public static IEntity Replace<T>(this IEntity entity, T component) where T : IComponent
		{
			return entity.ReplaceComponent(component);
		}

		public static IEntity RemoveComponent<T>(this IEntity entity) where T : IComponent
		{
			return entity.RemoveComponent(ComponentTypeId<T>.Value);
		}

		public static IEntity Remove<T>(this IEntity entity) where T : IComponent
		{
			return entity.RemoveComponent(ComponentTypeId<T>.Value);
		}

		public static bool HasComponent<T>(this IEntity entity) where T : IComponent
		{
			return entity.HasComponent(ComponentTypeId<T>.Value);
		}

		public static bool Has<T>(this IEntity entity) where T : IComponent
		{
			return entity.HasComponent(ComponentTypeId<T>.Value);
		}

		public static bool Has(this IEntity entity, int componentTypeId)
		{
			return entity.HasComponent(componentTypeId);
		}

		public static T GetComponent<T>(this IEntity entity) where T : IComponent
		{
			return (T)entity.GetComponent(ComponentTypeId<T>.Value);
		}

		public static T Get<T>(this IEntity entity) where T : IComponent
		{
			return (T)entity.GetComponent(ComponentTypeId<T>.Value);
		}

		public static T Get<T>(this IEntity entity, int componentTypeId) where T : IComponent
		{
			return (T)entity.GetComponent(componentTypeId);
		}
	}
}
