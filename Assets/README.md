# Architecture

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