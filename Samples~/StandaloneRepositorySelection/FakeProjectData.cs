using Deucarian.CoreState;

namespace Deucarian.CoreState.Samples.StandaloneRepositorySelection
{
    public sealed class FakeProjectData : IIdentifiable<int>
    {
        public FakeProjectData(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
