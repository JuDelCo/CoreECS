
namespace Ju.ECS
{
	public delegate void EntityComponentChangedEvent(IEntity entity, int componentTypeId);
	public delegate void EntityComponentReplacedEvent(IEntity entity, int componentTypeId);
	public delegate void EntityEvent(IEntity entity);

	public interface IEntity
	{
		event EntityComponentChangedEvent OnComponentAdded;
		event EntityComponentReplacedEvent OnComponentReplaced;
		event EntityComponentChangedEvent OnComponentRemoved;
		event EntityEvent OnRelease;
		event EntityEvent OnDestroy;

		void AddComponent(int componentTypeId, int componentPoolIndex);
		void ReplaceComponent(int componentTypeId);
		void RemoveComponent(int componentTypeId);
		bool HasComponent(int componentTypeId);
		int GetComponentPoolIndex(int componentTypeId);

		int GetTotalComponents();
		void RemoveAllComponents();

		uint GetUuid();
		bool IsEnabled();
		void Destroy();
		void Retain();
		void Release();

		void Reactivate();
		int GetRetainCount();
		void InternalDestroy();
	}
}
