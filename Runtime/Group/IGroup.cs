using System.Collections.Generic;

namespace Ju.ECS
{
	public delegate void GroupChangedEvent(IGroup group, IEntity entity);
	public delegate void GroupUpdatedEvent(IGroup group, IEntity entity);

	public interface IGroup
	{
		event GroupChangedEvent OnEntityAdded;
		event GroupUpdatedEvent OnEntityUpdated;
		event GroupChangedEvent OnEntityRemoved;

		int GetCount();

		List<IEntity> GetEntities();
		IEntity GetSingleEntity();

		IMatcher GetMatcher();

		void HandleEntitySilently(IEntity entity);
		GroupChangedEvent HandleEntity(IEntity entity);
		void UpdateEntity(IEntity entity);
	}
}
