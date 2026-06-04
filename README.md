# JorisHoef Core State

Small, standalone data state helpers for Unity packages and projects.

This package provides:

- `IIdentifiable<TKey>`
- `IReadOnlyRepository<TKey, T>`
- `IRepository<TKey, T>`
- `Repository<TKey, T>`
- `ISelectionService<TKey, T>`
- `SelectionService<TKey, T>`
- `SelectionChangedEventArgs<TKey, T>`
- `SelectionChangeMode`

The package has no UI, API, session, service locator, scene, `GameObject`, `MonoBehaviour`, or `UnityEngine` dependency.

## Usage

```csharp
using JorisHoef.Core.State;

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

## Repository

`Repository<TKey, T>` stores items by `IIdentifiable<TKey>.Id`.

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

## Selection

`SelectionService<TKey, T>` is constructed with an `IReadOnlyRepository<TKey, T>`.

It only selects keys that exist in the repository. If the selected item is removed, or the repository is cleared, the selection clears automatically. Replacing an item in the repository keeps selection valid by key.

```csharp
if (selection.TrySelect("project-1"))
{
    ProjectData selected = selection.SelectedItem;
}

selection.Clear(SelectionChangeMode.Programmatic);
```
