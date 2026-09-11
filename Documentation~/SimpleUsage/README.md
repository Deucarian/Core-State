# Simple usage

Copy the reference example into your project, add SimpleUsageExample and assign its scoped host references. Its serialized definition fields use the same typed keys as code.

Core State already accepts typed repository keys. This sample adds a small project-owned Inventory entry point with an enum definition and owner-issued item identity. Two inventories remain separate. No domain-specific inventory service or global registry is added to the package runtime. Serialize ItemKind for a dropdown when selecting which kind to create; created items receive runtime identities.

Definitions are authored once in SampleDefinitions.cs where applicable; the caller never invents an ID. Replace the sample set with your project's central definitions. A selected key proves its identity and payload type; startup still needs to bind that definition in the correct scope. Missing configuration reports how to fix it. Dynamic targets and choices are issued by their owner instead of selected from a definition dropdown.
```csharp
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
```
