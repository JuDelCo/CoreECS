
namespace Ju.ECS
{
	public static class ICollectorExtensions
	{
		public static ICollector CreateCollector(this IContext context, IMatcher matcher, GroupEvent groupEvent = GroupEvent.Added)
		{
			return new Collector(context.GetGroup(matcher), groupEvent);
		}
	}
}
