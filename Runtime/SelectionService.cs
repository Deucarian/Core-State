using System;
using System.Collections.Generic;

namespace Deucarian.CoreState
{
    public sealed class SelectionService<TKey, T> : ISelectionService<TKey, T>, IDisposable
    {
        private readonly IReadOnlyRepository<TKey, T> _repository;
        private bool _hasSelection;
        private TKey _selectedKey;
        private bool _isDisposed;

        public SelectionService(IReadOnlyRepository<TKey, T> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
            _repository.ItemRemoved += OnRepositoryItemRemoved;
            _repository.Cleared += OnRepositoryCleared;
        }

        public event EventHandler<SelectionChangedEventArgs<TKey, T>> SelectionChanged;

        public bool HasSelection
        {
            get { return _hasSelection; }
        }

        public TKey SelectedKey
        {
            get { return _hasSelection ? _selectedKey : default(TKey); }
        }

        public T SelectedItem
        {
            get
            {
                if (!_hasSelection)
                {
                    return default(T);
                }

                T item;
                return _repository.TryGet(_selectedKey, out item) ? item : default(T);
            }
        }

        public void Select(TKey key, SelectionChangeMode mode = SelectionChangeMode.Manual)
        {
            ThrowIfDisposed();
            if (!TrySelect(key, mode))
            {
                throw new KeyNotFoundException("The selected key does not exist in the repository.");
            }
        }

        public bool TrySelect(TKey key, SelectionChangeMode mode = SelectionChangeMode.Manual)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(key, null))
            {
                return false;
            }

            T selectedItem;
            if (!_repository.TryGet(key, out selectedItem))
            {
                return false;
            }

            if (_hasSelection && EqualityComparer<TKey>.Default.Equals(_selectedKey, key))
            {
                return true;
            }

            bool hadPreviousSelection = _hasSelection;
            TKey previousKey = SelectedKey;
            T previousItem = SelectedItem;

            _selectedKey = key;
            _hasSelection = true;

            RaiseSelectionChanged(
                hadPreviousSelection,
                previousKey,
                previousItem,
                true,
                _selectedKey,
                selectedItem,
                mode);

            return true;
        }

        public void Clear(SelectionChangeMode mode = SelectionChangeMode.Manual)
        {
            ThrowIfDisposed();
            ClearInternal(mode, SelectedItem);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _repository.ItemRemoved -= OnRepositoryItemRemoved;
            _repository.Cleared -= OnRepositoryCleared;
            _isDisposed = true;
        }

        private void OnRepositoryItemRemoved(TKey key, T item)
        {
            if (_isDisposed)
            {
                return;
            }

            if (!_hasSelection || !EqualityComparer<TKey>.Default.Equals(_selectedKey, key))
            {
                return;
            }

            ClearInternal(SelectionChangeMode.RepositoryChanged, item);
        }

        private void OnRepositoryCleared()
        {
            if (_isDisposed)
            {
                return;
            }

            Clear(SelectionChangeMode.RepositoryChanged);
        }

        private void ClearInternal(SelectionChangeMode mode, T previousItem)
        {
            if (!_hasSelection)
            {
                return;
            }

            TKey previousKey = _selectedKey;

            _selectedKey = default(TKey);
            _hasSelection = false;

            RaiseSelectionChanged(
                true,
                previousKey,
                previousItem,
                false,
                default(TKey),
                default(T),
                mode);
        }

        private void RaiseSelectionChanged(
            bool hadPreviousSelection,
            TKey previousKey,
            T previousItem,
            bool hasSelection,
            TKey selectedKey,
            T selectedItem,
            SelectionChangeMode mode)
        {
            SelectionChanged?.Invoke(
                this,
                new SelectionChangedEventArgs<TKey, T>(
                    hadPreviousSelection,
                    previousKey,
                    previousItem,
                    hasSelection,
                    selectedKey,
                    selectedItem,
                    mode));
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
