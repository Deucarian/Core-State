using UnityEngine;

namespace Deucarian.CoreState.Samples.SimpleUsage
{
    public sealed class SimpleUsageExample : MonoBehaviour
    {
        private readonly Inventory inventory = new Inventory();
        public InventoryItem AddSword() => inventory.Add(ItemKind.Sword);
        public bool Remove(InventoryItem item) => inventory.Remove(item);
    }
}
