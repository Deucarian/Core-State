using UnityEngine;

namespace Deucarian.CoreState.Samples.SimpleUsage
{
    public sealed class SimpleUsageExample : MonoBehaviour
    {
        [SerializeField] private ItemKind kind = ItemKind.Sword;
        private Inventory inventory;
        public void Configure(Inventory value) => inventory = value ?? throw new System.ArgumentNullException(nameof(value));
        public InventoryItem AddItem() => Inventory.Add(kind);
        public bool Remove(InventoryItem item) => Inventory.Remove(item);
        private Inventory Inventory => inventory ?? throw new System.InvalidOperationException("Configure this caller with the player's existing Inventory during startup.");
    }
}
