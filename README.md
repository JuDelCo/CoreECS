Core ECS
=====================

Core ECS is a deterministic lightweight ECS framework.

It is heavily inspired in Entitas, but with some differences:

- Is deterministic (does not rely on hashtables)
- Does not need a code generator (no reflection, less problems)
- Easier to start using it
- No GC collections

Currently is in work in progress state.

This package will follow a [semantic versioning](http://semver.org/).

Any feedback is welcome !


See also
=====================

- [Core](https://github.com/JuDelCo/Core) - Core package base
- [Core Unity](https://github.com/JuDelCo/CoreUnity) - Core services extension for Unity3D


Documentation
=====================

Work in progress...


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
