using System.Collections.Generic;

namespace JorisHoef.Core.State
{
    public interface IRepository<TKey, T> : IReadOnlyRepository<TKey, T>
        where T : IIdentifiable<TKey>
    {
        void AddOrUpdate(T item);
        void AddOrUpdateMany(IEnumerable<T> items);
        bool Remove(TKey key);
        void Clear();
    }
}
