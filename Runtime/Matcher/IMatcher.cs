using System;
using System.Collections.Generic;

namespace Ju.ECS
{
	public interface IMatcher
	{
		bool Matches(IEntity entity);
		IMatcher AllOf(List<Type> types);
		IMatcher AnyOf(List<Type> types);
		IMatcher NoneOf(List<Type> types);
		List<Type> GetTypes();
	}
}
