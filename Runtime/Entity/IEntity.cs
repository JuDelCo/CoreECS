using System;

namespace Ju.ECS
{
	public delegate void EntityComponentChangedEvent(IEntity entity, IComponent component);
	public delegate void EntityComponentReplacedEvent(IEntity entity, IComponent previousComponent, IComponent newComponent);

	public interface IEntity
	{
		event EntityComponentChangedEvent OnComponentAdded;
		event EntityComponentReplacedEvent OnComponentReplaced;
		event EntityComponentChangedEvent OnComponentRemoved;

		T CreateComponent<T>() where T : IComponent, new();

		IEntity AddComponent(IComponent component);
		IEntity ReplaceComponent(IComponent component);
		IEntity RemoveComponent(Type type);
		bool HasComponent(Type type);
		IComponent GetComponent(Type type);

		int GetTotalComponents();
		void RemoveAllComponents();
		uint GetUuid();
	}
}
