using System;

namespace JorisHoef.Core.State
{
    public interface ISelectionService<TKey, T>
    {
        event EventHandler<SelectionChangedEventArgs<TKey, T>> SelectionChanged;

        bool HasSelection { get; }
        TKey SelectedKey { get; }
        T SelectedItem { get; }

        void Select(TKey key, SelectionChangeMode mode = SelectionChangeMode.Manual);
        bool TrySelect(TKey key, SelectionChangeMode mode = SelectionChangeMode.Manual);
        void Clear(SelectionChangeMode mode = SelectionChangeMode.Manual);
    }
}
