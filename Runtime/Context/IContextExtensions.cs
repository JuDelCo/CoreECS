using System.Collections.Generic;

namespace Ju.ECS
{
	public static class IContextExtensions
	{
		public static List<IEntity> GetEntities(this IContext context, IMatcher matcher)
		{
			return context.GetGroup(matcher).GetEntities();
		}
	}
}
