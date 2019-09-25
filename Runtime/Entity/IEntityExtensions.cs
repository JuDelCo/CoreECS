
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
			return entity.RemoveComponent(typeof(T));
		}

		public static IEntity Remove<T>(this IEntity entity) where T : IComponent
		{
			return entity.RemoveComponent(typeof(T));
		}

		public static bool HasComponent<T>(this IEntity entity) where T : IComponent
		{
			return entity.HasComponent(typeof(T));
		}

		public static bool Has<T>(this IEntity entity) where T : IComponent
		{
			return entity.HasComponent(typeof(T));
		}

		public static T GetComponent<T>(this IEntity entity) where T : IComponent
		{
			return (T)entity.GetComponent(typeof(T));
		}

		public static T Get<T>(this IEntity entity) where T : IComponent
		{
			return (T)entity.GetComponent(typeof(T));
		}
	}
}
