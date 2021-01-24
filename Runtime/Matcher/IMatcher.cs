// SPDX-License-Identifier: MIT
// Copyright (c) 2019-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;

namespace Ju.ECS
{
	public interface IMatcher
	{
		bool Matches(IEntity entity);
		IMatcher AllOf(List<int> componentTypeIds);
		IMatcher AnyOf(List<int> componentTypeIds);
		IMatcher NoneOf(List<int> componentTypeIds);
		List<int> GetTypes();
	}
}
