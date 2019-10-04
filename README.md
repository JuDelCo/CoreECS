Core ECS
=====================

Core ECS is a deterministic lightweight ECS framework.

It's heavily inspired in Entitas, but with some differences:

- It's deterministic (does not rely on hashtables)
- Does not use code generators (no reflection, less problems)
- Easier to start using it (more straightforward, see documentation below)
- No GC collections (when reusing destroyed entities)
- Similar perfomance (10~15% less, but handles well all requirements for a game)

This package will follow a [semantic versioning](http://semver.org/).

Any feedback is welcome !


See also
=====================

- [Core](https://github.com/JuDelCo/Core) - Core package base
- [Core Math](https://github.com/JuDelCo/CoreMath) - Simple linear algebra math library
- [Core Unity](https://github.com/JuDelCo/CoreUnity) - Core services extension for Unity3D


Documentation
=====================

Note: All components should be structs for best performance and memory efficiency.

#### Defining components

```csharp
public struct Speed : IComponent
{
	public float value;

	public Speed(float value) // Optional, syntactic sugar for later
	{
		this.value = value;
	}
}
```

#### Create a Context

```csharp
// IMPORTANT: You need to pass the count of component types you will use to the context
const int COMPONENT_TYPES_COUNT = 9;

var context = new Context(Constants.COMPONENT_TYPES_COUNT);
```

#### Creating entities

```csharp
var entity = context.CreateEntity();

entity.Add(new Speed(2f));
```

#### Entity management

```csharp
entity.Add(new Position(3, 7));
entity.Replace(new Health(10));
entity.Remove<Speed>();

var hasSpeed = entity.Has<Speed>();
var healthData = entity.Get<Health>();

// You can also chain methods!
context.CreateEntity()
	.Add(new Speed(2f))
	.Add(new Position(3, 7))
	.Replace(new Health(10)) // Note: Replace will add the component if doesn't have it yet
	.Remove<Speed>();
```

#### Group management

```csharp
// Returns a group containing always all entities with Position and Speed components.
var group = context.GetGroup(MatcherGenerator.AllOf<Position, Speed>());

var entities = group.GetEntities();

foreach (var e in entities)
{
	// ...
}
```

#### Execute systems

```csharp
// You can also use IInitializeSystem, ICleanupSystem and ITearDownSystem systems
public class MovementSystem : IExecuteSystem
{
	private IGroup group;

	public MovementSystem(IContext context)
	{
		// You can also use a shorthand for AllOf components!
		// This is equivalent to: GetGroup(MatcherGenerator.AllOf<Position, Speed>())
		group = context.GetGroup<Position, Speed>();
	}

	public void Execute()
	{
		foreach (var e in group.GetEntities())
		{
			var position = e.Get<Position>();
			var speed = e.Get<Speed>().value;
			position.x += speed;

			e.Replace(position);
		}
	}
}
```

#### Reactive systems

```csharp
public class RenderPositionSystem : ReactiveSystem
{
	private IContext context;
	private IGroup group;

	public RenderPositionSystem(IContext context) : base(context)
	{
		this.context = context;
	}

	protected override ICollector GetTrigger(IContext context)
	{
		return context.GetGroup<Position, View>().CreateCollector(GroupEvent.Added);
	}

	protected override bool Filter(IEntity entity)
	{
		// Yes, you can use multiple types in Has<T> method !
		return entity.Has<Position, View>();
	}

	protected override void Execute(List<IEntity> entities)
	{
		foreach (var e in entities)
		{
			var position = e.Get<Position>();
			e.Get<View>().gameObject.transform.position = new Vector3(position.x, position.y, position.z);
		}
	}
}
```


Install
=====================

If you are using Unity, update the dependencies in the ```/Packages/manifest.json``` file in your project folder with:

```
	"com.judelco.core.ecs": "https://github.com/JuDelCo/CoreECS.git",
```

otherwise, use this package as it is in native C# applications, as it doesn't have dependencies with Unity.


The MIT License (MIT)
=====================

Copyright © 2019 Juan Delgado (JuDelCo)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
