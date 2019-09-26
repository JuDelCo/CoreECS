using System.Collections.Generic;

namespace Ju.ECS
{
	public class EntityEqualityComparer : IEqualityComparer<IEntity>
	{
		public bool Equals(IEntity left, IEntity right)
		{
			return left.GetUuid() == right.GetUuid();
		}

		public int GetHashCode(IEntity entity)
		{
			return (int)entity.GetUuid();
		}
	}
}
