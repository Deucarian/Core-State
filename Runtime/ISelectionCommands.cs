namespace Deucarian.CoreState
{
    public interface ISelectionCommands<TKey>
    {
        void Select(TKey key, SelectionChangeMode mode = SelectionChangeMode.Manual);
        bool TrySelect(TKey key, SelectionChangeMode mode = SelectionChangeMode.Manual);
        void Clear(SelectionChangeMode mode = SelectionChangeMode.Manual);
    }
}
