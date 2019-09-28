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
- [Core Unity](https://github.com/JuDelCo/CoreUnity) - Core services extension for Unity3D


Documentation
=====================

#### Overview

NOTE: All components should be structs for best performance and memory efficiency.

Basic data architecture diagram:

```
+------------------+
|     Context      |
|------------------|
|    e       e     |      +-----------+
|        e     e---|----> |  Entity   |
|  e        e      |      |-----------|
|     e  e       e |      | Component |
| e            e   |      |           |      +-----------+
|    e     e       |      | Component-|----> | Component |
|  e    e     e    |      |           |      |-----------|
|    e      e    e |      | Component |      |   Data    |
+------------------+      +-----------+      +-----------+
  |
  |
  |     +-------------+  Groups:
  |     |      e      |  Subsets of entities in the context
  |     |   e     e   |  for blazing fast querying
  +---> |        +------------+
        |     e  |    |       |
        |  e     | e  |  e    |
        +--------|----+    e  |
                 |     e      |
                 |  e     e   |
                 +------------+
```

#### Define components

```csharp
public struct Speed : IComponent
{
	public float value;

	public Speed(float value) // Optional, syntactic sugar for later
	{
		this.value = value;
	}
}

// In other source file...
var entity = context.CreateEntity();
entity.Add(new Speed(2f));
```

#### Component management

```csharp
entity.Add(new Position(3, 7));
entity.Replace(new Health(10));
entity.Remove<Speed>();

var hasSpeed = entity.Has<Speed>();
var healthData = entity.Get<Health>();

// You can also chain methods !
context.CreateEntity()
	.Add(new Speed(2f))
	.Add(new Position(3, 7))
	.Replace(new Health(10)) // Replace will add the component if doesn't have one yet
	.Remove<Speed>();
```

#### Context creation

```csharp
// IMPORTANT: You need to pass the count of component types you will use to the context
const int TYPE_COUNT = 25;

var context = new Context(Constants.TYPE_COUNT);

var entity = context.CreateEntity();
entity.Add(new Speed());

// Returns all entities having Position and Speed components.
var entities = context.GetEntities(MatcherGen.AllOf<Position, Speed>());

foreach (var e in entities)
{
	// ...
}
```

#### Group events

```csharp
context.GetGroup(MatcherGen.AllOf<Position>()).OnEntityAdded += (group, entity) =>
{
	// ...
};

context.GetGroup(MatcherGen.AllOf<Position>()).OnEntityRemoved += (group, entity) =>
{
	// ...
};
```

#### Collectors

```csharp
var group = context.GetGroup(MatcherGen.AllOf<Position>());
var collector = group.CreateCollector(GroupEvent.Added);

foreach (var e in collector.GetCollectedEntities())
{
	// ...
}

collector.ClearCollectedEntities();
```

#### Execute systems

```csharp
public class MovementSystem : IExecuteSystem
{
	private IGroup group;

	public MovementSystem(IContext context)
	{
		group = context.GetGroup(MatcherGen.AllOf<Position, Speed>());
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
		return context.GetGroup(MatcherGen.AllOf<Position, View>()).CreateCollector(GroupEvent.Added);
	}

	protected override bool Filter(IEntity entity)
	{
		return entity.Has<View>();
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
