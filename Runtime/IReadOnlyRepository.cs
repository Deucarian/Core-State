using System;
using System.Collections.Generic;

namespace JorisHoef.Core.State
{
    public interface IReadOnlyRepository<TKey, T>
    {
        event Action<TKey, T> ItemAdded;
        event Action<TKey, T> ItemUpdated;
        event Action<TKey, T> ItemRemoved;
        event Action Cleared;

        int Count { get; }
        IReadOnlyCollection<T> Items { get; }

        bool ContainsKey(TKey key);
        bool TryGet(TKey key, out T item);
    }
}
