using System;

namespace Deucarian.CoreState
{
    public sealed class SelectionChangedEventArgs<TKey, T> : EventArgs
    {
        public SelectionChangedEventArgs(
            bool hadPreviousSelection,
            TKey previousKey,
            T previousItem,
            bool hasSelection,
            TKey selectedKey,
            T selectedItem,
            SelectionChangeMode mode)
        {
            HadPreviousSelection = hadPreviousSelection;
            PreviousKey = previousKey;
            PreviousItem = previousItem;
            HasSelection = hasSelection;
            SelectedKey = selectedKey;
            SelectedItem = selectedItem;
            Mode = mode;
        }

        public bool HadPreviousSelection { get; }
        public TKey PreviousKey { get; }
        public T PreviousItem { get; }
        public bool HasSelection { get; }
        public TKey SelectedKey { get; }
        public T SelectedItem { get; }
        public SelectionChangeMode Mode { get; }
    }
}
