using System;
using System.Collections.Generic;

namespace Deucarian.CoreState
{
    public sealed class Repository<TKey, T> : IRepository<TKey, T>
        where T : IIdentifiable<TKey>
    {
        private readonly Dictionary<TKey, T> _items;

        public Repository()
            : this(null)
        {
        }

        public Repository(IEqualityComparer<TKey> comparer)
        {
            _items = new Dictionary<TKey, T>(comparer);
        }

        public event Action<TKey, T> ItemAdded;
        public event Action<TKey, T> ItemUpdated;
        public event Action<TKey, T> ItemRemoved;
        public event Action Cleared;

        public int Count
        {
            get { return _items.Count; }
        }

        public IReadOnlyCollection<T> Items
        {
            get { return _items.Values; }
        }

        public void AddOrUpdate(T item)
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException(nameof(item));
            }

            TKey key = item.Id;
            ValidateKey(key);

            bool exists = _items.ContainsKey(key);
            _items[key] = item;

            if (exists)
            {
                ItemUpdated?.Invoke(key, item);
            }
            else
            {
                ItemAdded?.Invoke(key, item);
            }
        }

        public void AddOrUpdateMany(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (T item in items)
            {
                AddOrUpdate(item);
            }
        }

        public bool Remove(TKey key)
        {
            ValidateKey(key);

            T item;
            if (!_items.TryGetValue(key, out item))
            {
                return false;
            }

            _items.Remove(key);
            ItemRemoved?.Invoke(key, item);
            return true;
        }

        public void Clear()
        {
            if (_items.Count == 0)
            {
                return;
            }

            _items.Clear();
            Cleared?.Invoke();
        }

        public bool ContainsKey(TKey key)
        {
            ValidateKey(key);
            return _items.ContainsKey(key);
        }

        public bool TryGet(TKey key, out T item)
        {
            ValidateKey(key);
            return _items.TryGetValue(key, out item);
        }

        private static void ValidateKey(TKey key)
        {
            if (ReferenceEquals(key, null))
            {
                throw new ArgumentNullException(nameof(key));
            }
        }
    }
}
