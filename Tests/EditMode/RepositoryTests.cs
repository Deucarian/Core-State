using System.Collections.Generic;
using NUnit.Framework;

namespace JorisHoef.Core.State.Tests
{
    public sealed class RepositoryTests
    {
        [Test]
        public void RepositoryCanAddUpdateGetRemoveAndClear()
        {
            var repository = new Repository<string, TestItem>();
            var first = new TestItem("one", "First");
            var updatedFirst = new TestItem("one", "Updated first");

            repository.AddOrUpdate(first);

            Assert.AreEqual(1, repository.Count);
            Assert.IsTrue(repository.ContainsKey("one"));
            Assert.IsTrue(repository.TryGet("one", out TestItem found));
            Assert.AreSame(first, found);

            repository.AddOrUpdate(updatedFirst);

            Assert.AreEqual(1, repository.Count);
            Assert.IsTrue(repository.TryGet("one", out found));
            Assert.AreSame(updatedFirst, found);

            repository.AddOrUpdateMany(new[]
            {
                new TestItem("two", "Second"),
                new TestItem("three", "Third")
            });

            Assert.AreEqual(3, repository.Count);
            Assert.IsTrue(repository.Remove("two"));
            Assert.IsFalse(repository.ContainsKey("two"));
            Assert.IsFalse(repository.Remove("missing"));

            repository.Clear();

            Assert.AreEqual(0, repository.Count);
        }

        [Test]
        public void RepositoryEventsFireCorrectly()
        {
            var repository = new Repository<string, TestItem>();
            var addedKeys = new List<string>();
            var updatedKeys = new List<string>();
            var removedKeys = new List<string>();
            int clearedCount = 0;

            repository.ItemAdded += (key, _) => addedKeys.Add(key);
            repository.ItemUpdated += (key, _) => updatedKeys.Add(key);
            repository.ItemRemoved += (key, _) => removedKeys.Add(key);
            repository.Cleared += () => clearedCount++;

            repository.AddOrUpdate(new TestItem("one", "First"));
            repository.AddOrUpdate(new TestItem("one", "Updated first"));
            repository.AddOrUpdate(new TestItem("two", "Second"));
            repository.Remove("one");
            repository.Clear();

            CollectionAssert.AreEqual(new[] { "one", "two" }, addedKeys);
            CollectionAssert.AreEqual(new[] { "one" }, updatedKeys);
            CollectionAssert.AreEqual(new[] { "one" }, removedKeys);
            Assert.AreEqual(1, clearedCount);
        }

        private sealed class TestItem : IIdentifiable<string>
        {
            public TestItem(string id, string name)
            {
                Id = id;
                Name = name;
            }

            public string Id { get; }
            public string Name { get; }
        }
    }
}
