# Architecture Decision

## Single Grid Manager Class

### Advantages:

* The most straight-forward approach.

### Disadvantages:

* Not testable
* Gigantic class with too much responsibilities
* Hard to maintain

## MVC

### Advantages:

* Simpler approach to separate view and business logic from each other
* Testable
* Easy to maintain

### Disadvantages:

* Controller -> View dependency is not very desirable for this problem. Controller can drive the view by calling the methods on it, but it also needs to listen to inputs / commands from somewhere (`ITapMatchInteractions`). Having references from Controller to both view and an aggregated interaction listener may not make sense because view is already an interaction provider.

```csharp
interface IInteractionProvider { /* Events */ }

class EditorGridInteractionProvider : IInteractionProvider {}

class GridView : MonoBehaviour, IInteractionProvider {}

class GridController { /* ref. to GridView, IInteractionProvider[] */ }

class GridModel {}
```

## MVVM

### Advantages:

* Testable
* Easy to maintain
* View -> Controller dependency makes more sense when an interaction provider is introduced:

```csharp
interface IInteractionProvider { /* Events */ }

class EditorGridInteractionProvider : IInteractionProvider {}

class GridView : MonoBehaviour, IInteractionProvider { /* ref. to GridViewModel */ }

class GridViewModel { /* ref. to IInteractionProvider[] */ }

class GridModel {}
```

**This project is developed with MVVM.**

# Assemblies, Classes and Dependencies

There are multiple assemblies in the project, each with different responsibilites.

![ItemTypeDependency](https://user-images.githubusercontent.com/3823941/200121194-5c19fa3f-8c1b-4e3b-bfec-3bb493c4f2a7.jpg)
![MainAssemblies](https://user-images.githubusercontent.com/3823941/200121197-2658fa9b-41d4-4c43-a034-5b7a52f821d6.jpg)
![MainClassDependency](https://user-images.githubusercontent.com/3823941/200121198-3eb6b159-bfbd-47fc-b8ef-3adf1dcc0fc3.jpg)
![SearchDependency](https://user-images.githubusercontent.com/3823941/200121199-7015bbdf-6822-423e-b885-ecbad95d909c.jpg)
![TestAssembly](https://user-images.githubusercontent.com/3823941/200121200-96a11264-355e-4369-8a04-1079a678bea2.jpg)


```csharp
class GridViewModel {
	IInteractionProvider[] interactions;
	event AddedGridItems;
	event DestroyedGridItems;
	event ShiftedGridItems;
}

class GridView : MonoBehaviour, IInteractionSource {
	GridViewModel viewModel;
	event GridItemSelected;
}

class EditorGridInteractionProvider : IInteractionProvider {
	event GridItemSelected;
}

class GameInitializer : MonoBehaviour {
	// Init view
	// Init interaction sources
	// Init view-model with interaction providers (sources)
}
```
