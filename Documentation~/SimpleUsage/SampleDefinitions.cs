namespace Deucarian.CoreState.Samples.SimpleUsage
{
    public enum ItemKind { Sword, Potion }
    public sealed class InventoryItem : IIdentifiable<System.Guid>
    {
        internal InventoryItem(ItemKind kind) { Id = System.Guid.NewGuid(); Kind = kind; }
        public System.Guid Id { get; }
        public ItemKind Kind { get; }
    }
    // Project-owned domain example; Core State remains a generic repository package.
    public sealed class Inventory
    {
        private readonly Repository<System.Guid, InventoryItem> items = new Repository<System.Guid, InventoryItem>();
        public InventoryItem Add(ItemKind kind)
        {
            if (kind != ItemKind.Sword && kind != ItemKind.Potion) throw new System.ArgumentOutOfRangeException(nameof(kind));
            var item = new InventoryItem(kind);
            items.AddOrUpdate(item);
            return item;
        }
        public bool Remove(InventoryItem item) => item != null && items.TryGet(item.Id, out var current) &&
            ReferenceEquals(current, item) && items.Remove(item.Id);
    }
}
