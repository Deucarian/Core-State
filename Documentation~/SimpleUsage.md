# Simple usage

Copy the reference example into your project, add SimpleUsageExample and assign its scoped host references. Its serialized definition fields use the same typed keys as code.

Core State already accepts typed repository keys. Configure the caller once with the existing Inventory owned by that player; the caller borrows it. ItemKind works as a serialized dropdown or a named code value. This sample adds a small project-owned Inventory entry point with an enum definition and owner-issued item identity. Two inventories remain separate. No domain-specific inventory service or global registry is added to the package runtime. Serialize ItemKind for a dropdown when selecting which kind to create; created items receive runtime identities.

Definitions are authored once in SampleDefinitions.cs where applicable; the caller never invents an ID. Replace the sample set with your project's central definitions. A selected key proves its identity and payload type; startup still needs to bind that definition in the correct scope. Missing configuration reports how to fix it. Dynamic targets and choices are issued by their owner instead of selected from a definition dropdown.
```csharp
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
```
