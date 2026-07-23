namespace Deucarian.CoreState
{
    public interface ISelectionService<TKey, T> :
        IReadOnlySelection<TKey, T>,
        ISelectionCommands<TKey>
    {
    }
}
