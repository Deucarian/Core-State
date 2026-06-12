namespace Deucarian.CoreState
{
    public interface IIdentifiable<out TKey>
    {
        TKey Id { get; }
    }
}
