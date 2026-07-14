# Deucarian Core State

## What this is

`com.deucarian.core-state` is a small, standalone runtime package for keeping keyed data and current selection state in plain C#.

It provides repository and selection primitives that can be reused by Unity packages, tools, or game code without taking dependencies on UI, networking, sessions, service locators, scenes, `GameObject`, `MonoBehaviour`, or `UnityEngine`.

Current package version: `1.0.1`.

## When to use it

- You need a plain C# keyed repository for runtime data.
- You need a single-selection service that stays valid as repository data changes.
- You need reusable repository/selection contracts without UI, networking, scenes, or service locators.
- You want another package to compose Core State rather than owning duplicate selection primitives.

## When not to use it

- Do not use Core State for UI binding, world selection, persistence, networking, undo/redo, or domain-specific state.
- Do not add Logging, Common, Editor, Diagnostics, or runtime Unity object dependencies unless governance approves a direct need.
- Do not use Core State as a service locator or project-wide architecture framework.

## Install

Stable:

```json
"com.deucarian.core-state": "https://github.com/Deucarian/Core-State.git#main"
```

Development:

```json
"com.deucarian.core-state": "https://github.com/Deucarian/Core-State.git#develop"
```

Core State has no package dependencies.

## Unity compatibility

Requires Unity 2021.3 or newer.

## 60-second quick start

`Repository<TKey, T>` stores items by key. The item type must implement `IIdentifiable<TKey>` so the repository can read each item's stable `Id`.

`SelectionService<TKey, T>` tracks one selected key against an `IReadOnlyRepository<TKey, T>`. It only selects keys that exist in the repository. If the selected item is removed, or the repository is cleared, the selection clears automatically.

Repository replacement is key based. Updating an item with the same key keeps the selection valid.

## Public API map

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

## Limitations

- Repository storage is in memory only. Persistence belongs in application code or another package.
- Selection is single-item selection by key, not multi-select.
- The package does not include UI bindings, serialization, networking, undo/redo, or thread-safety primitives.
- Null keys are rejected by repository operations; `TrySelect` returns `false` for null keys.

## Troubleshooting

- If selection clears unexpectedly, check whether the selected key was removed or the repository was cleared.
- If `TrySelect` returns `false`, confirm the key is non-null and currently exists in the repository.
- If a package needs UI, persistence, networking, or world-object selection, compose Core State from that owning package instead of adding those concerns here.

## Validation

Run the shared package validator from the repository root:

```powershell
python C:/Repositories/Package-Registry/Tools/deucarian_package_validator.py --registry-root C:/Repositories/Package-Registry --repository-root . --config deucarian-package.json
```

Run the package's EditMode tests in Unity after code or assembly definition changes.

Documentation-only updates should still pass:

```powershell
git diff --check
```

## Architecture / Contributor Notes

- [AGENTS.md](AGENTS.md) contains repository-specific ownership and Codex guidance.
- Deucarian architecture rules live in [Package Registry](https://github.com/Deucarian/Package-Registry/blob/develop/ARCHITECTURE.md).
- Capability ownership is tracked in [CAPABILITY_OWNERSHIP.md](https://github.com/Deucarian/Package-Registry/blob/develop/CAPABILITY_OWNERSHIP.md).

## License

MIT. See [LICENSE.md](LICENSE.md).
