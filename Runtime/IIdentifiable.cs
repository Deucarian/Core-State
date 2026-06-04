namespace JorisHoef.Core.State
{
    public interface IIdentifiable<out TKey>
    {
        TKey Id { get; }
    }
}
