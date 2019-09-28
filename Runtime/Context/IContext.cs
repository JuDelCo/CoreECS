using System.Collections.Generic;

namespace Ju.ECS
{
	public delegate void ContextEntityEvent(IContext context, IEntity entity);
	public delegate void ContextGroupEvent(IContext context, IGroup group);

	public interface IContext
	{
		event ContextEntityEvent OnEntityCreated;
		event ContextEntityEvent OnEntityDestroyed;
		event ContextGroupEvent OnGroupCreated;

		IEntity CreateEntity();

		void DestroyEntity(IEntity entity);
		void DestroyAllEntities();

		List<IEntity> GetEntities();
		IGroup GetGroup(IMatcher matcher);
		int GetEntityCount();
	}
}
