using System;
namespace Deucarian.CoreState.Samples.DefinitionWorkflow
{
    public sealed class SampleItem : IIdentifiable<Guid> { public Guid Id { get; } = Guid.NewGuid(); }
}
