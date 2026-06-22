# Deucarian Core State

## Overview

Core State is a small, standalone runtime package for keeping keyed data and current selection state in plain C#.

It provides repository and selection primitives that can be reused by Unity packages, tools, or game code without taking dependencies on UI, networking, sessions, service locators, scenes, `GameObject`, `MonoBehaviour`, or `UnityEngine`.

Package ID: `com.deucarian.core-state`

## Installation

Install the package through Unity Package Manager with a Git URL:

```json
{
  "dependencies": {
    "com.deucarian.core-state": "https://github.com/Deucarian/Core-State.git#main"
  }
}
```

For development builds, use:

```json
"com.deucarian.core-state": "https://github.com/Deucarian/Core-State.git#develop"
```

The package requires Unity `2021.3` or newer and has no package dependencies.

## Core Concepts

`Repository<TKey, T>` stores items by key. The item type must implement `IIdentifiable<TKey>` so the repository can read each item's stable `Id`.

`SelectionService<TKey, T>` tracks one selected key against an `IReadOnlyRepository<TKey, T>`. It only selects keys that exist in the repository. If the selected item is removed, or the repository is cleared, the selection clears automatically.

Repository replacement is key based. Updating an item with the same key keeps the selection valid.

## Public API

- `IIdentifiable<TKey>`: item contract containing `Id`.
- `IReadOnlyRepository<TKey, T>`: read-only repository view with count, items, lookup methods, and change events.
- `IRepository<TKey, T>`: mutable repository contract with add, update, remove, and clear operations.
- `Repository<TKey, T>`: default in-memory repository implementation.
- `ISelectionService<TKey, T>`: current selection contract.
- `SelectionService<TKey, T>`: default selection service implementation.
- `SelectionChangedEventArgs<TKey, T>`: previous and current selection data for selection-change listeners.
- `SelectionChangeMode`: identifies manual, programmatic, or repository-driven selection changes.

Basic repository and selection workflow:

```csharp
using Deucarian.CoreState;

public sealed class ProjectData : IIdentifiable<string>
{
    public ProjectData(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; }
    public string Name { get; }
}

var repository = new Repository<string, ProjectData>();
var selection = new SelectionService<string, ProjectData>(repository);

repository.AddOrUpdate(new ProjectData("project-1", "Main Project"));

selection.SelectionChanged += (_, args) =>
{
    if (args.HasSelection)
    {
        ProjectData selected = args.SelectedItem;
    }
};

selection.Select("project-1");
```

Repository operations:

```csharp
repository.ItemAdded += (key, item) => { };
repository.ItemUpdated += (key, item) => { };
repository.ItemRemoved += (key, item) => { };
repository.Cleared += () => { };

repository.AddOrUpdate(item);
repository.AddOrUpdateMany(items);
repository.TryGet(key, out var item);
repository.Remove(key);
repository.Clear();
```

Selection operations:

```csharp
if (selection.TrySelect("project-1"))
{
    ProjectData selected = selection.SelectedItem;
}

selection.Clear(SelectionChangeMode.Programmatic);
```

## Samples

The package contains one sample:

- `Standalone Repository Selection`: an IMGUI scene at `Samples~/StandaloneRepositorySelection/StandaloneRepositorySelection.unity`.

Open the scene and enter Play Mode. The sample creates fake project data, selects repository items by key, rejects a missing key, removes the selected item, clears the repository, and displays current repository and selection state. The sample has its own `MonoBehaviour` code; the Core State runtime remains pure C#.

## Integrations

Core State has no integration assembly and does not reference the other Deucarian packages.

It is intended to be composed by consumers. For example, project code can store data in `Repository<TKey, T>` and render it with UI Binding, but that composition is not built into this package.

The Package Installer can install Core State alongside UI Binding, but Core State itself remains standalone.

## Versioning

Current package version: `1.0.1`.

Branch strategy:

- `main`: stable package branch.
- `develop`: development package branch.

Use branch refs for active development and stable release tags when tags are available.

## Limitations

- Repository storage is in memory only. Persistence belongs in application code or another package.
- Selection is single-item selection by key, not multi-select.
- The package does not include UI bindings, serialization, networking, undo/redo, or thread-safety primitives.
- Null keys are rejected by repository operations; `TrySelect` returns `false` for null keys.

## Architecture / Contributor Notes

- [AGENTS.md](AGENTS.md) contains repository-specific ownership and Codex guidance.
- Deucarian architecture rules live in [Package Registry](https://github.com/Deucarian/Package-Registry/blob/develop/ARCHITECTURE.md).
- Capability ownership is tracked in [CAPABILITY_OWNERSHIP.md](https://github.com/Deucarian/Package-Registry/blob/develop/CAPABILITY_OWNERSHIP.md).
