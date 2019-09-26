
namespace Ju.ECS
{
	public abstract class IComponent
	{
		private int componentTypeId;

		protected IComponent()
		{
			componentTypeId = ComponentTypeId.Get(this.GetType());
		}

		public int GetTypeId()
		{
			return componentTypeId;
		}
	}
}
