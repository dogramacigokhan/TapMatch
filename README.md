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

---

# Assemblies, Classes and Dependencies

There are multiple assemblies in the project, each with different responsibilites.

## Main Assemblies
<img alt="MainAssemblies" src="https://user-images.githubusercontent.com/3823941/200121197-2658fa9b-41d4-4c43-a034-5b7a52f821d6.jpg">

Grid System contains the main logic with Views, ViewModels, Models and search algorithm. And the other assemblies provide required interfaces and implementations to setup and run the game.

For a such small project multiple assemblies are maybe not required, but this repository demonstrates how they can be used to separate concerns into multiple assemblies and also reduce the compile times when the project grows.

## Test Assembly

<img alt="TestAssembly" height="400" src="https://user-images.githubusercontent.com/3823941/200121200-96a11264-355e-4369-8a04-1079a678bea2.jpg">

There are play-mode (unit) tests in the project for finding the matches with depth-first search algorithm, and for the Grid ViewModel. With the help of MVVM, business logic is extracted from the view easily into the ViewModel so it can be tested easily without any Unity dependency.

There's also NSubstitute dependency to mock some interfaces to reduce the scope of testing.

There's currently also 100% code coverage for the ViewModel and DFS class:

![Coverage](https://user-images.githubusercontent.com/3823941/200122762-3f53074b-4e68-446d-b76b-8ea627927e05.jpg)

Overall coverage can be increased by adding tests for Grid View, and Grid Item views. Since they don't hold the business logic in them, a mocked ViewModel can be created to drive them and assert UI changes in the test easily.

## Item (Candy) Types
![ItemTypeDependency](https://user-images.githubusercontent.com/3823941/200121194-5c19fa3f-8c1b-4e3b-bfec-3bb493c4f2a7.jpg)

Item types are implemented using ScriptableObjects which hold _readonly_ settings in them. This data includes color, sprite type, item id, etc. to identify the item and define its visuals.

With the help of this approach, new item / candy types can be easily added without requiring any code change by just creating new scriptable objects and assigning them in the editor.

## Main Class Dependency
![MainClassDependency](https://user-images.githubusercontent.com/3823941/200121198-3eb6b159-bfbd-47fc-b8ef-3adf1dcc0fc3.jpg)

* With this approach, ViewModel holds the business logic and does not care about the view and Unity related logic.
* View does not care about the business logic, only listens to the ViewModel to adjust itself.
* Model classes hold data that can be accessed and modified through ViewModel.
* `GridViewModel` depends on `IInteractionProvider` abstraction rather than a certain interaction provider (e.g. the view) so new interaction providers can be defined easily, e.g. `EditorGridInteractionProvider` to drive interactions in Unity Editor.

This approach makes testing extremely easy, because the concerns are separated into different classes and the dependencies can be easily mocked.

## DFS Search Dependency

<img alt="DFS Search Dependency" height="300" src="https://user-images.githubusercontent.com/3823941/200121199-7015bbdf-6822-423e-b885-ecbad95d909c.jpg">

Similar to interaction providers, search algorithms are also abstracted behind an interface (`IGridMatchFinder`) so the algorithm can be replaced without modifying the dependants directly.
