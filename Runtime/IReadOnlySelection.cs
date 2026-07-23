using System;

namespace Deucarian.CoreState
{
    public interface IReadOnlySelection<TKey, T>
    {
        event EventHandler<SelectionChangedEventArgs<TKey, T>> SelectionChanged;

        bool HasSelection { get; }
        TKey SelectedKey { get; }
        T SelectedItem { get; }
    }
}
