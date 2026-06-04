using System.Collections.Generic;
using NUnit.Framework;

namespace JorisHoef.Core.State.Tests
{
    public sealed class SelectionServiceTests
    {
        [Test]
        public void SelectionCanSelectExistingItem()
        {
            var repository = new Repository<string, TestItem>();
            var item = new TestItem("one", "First");
            repository.AddOrUpdate(item);

            var selection = new SelectionService<string, TestItem>(repository);

            selection.Select("one");

            Assert.IsTrue(selection.HasSelection);
            Assert.AreEqual("one", selection.SelectedKey);
            Assert.AreSame(item, selection.SelectedItem);
        }

        [Test]
        public void SelectionCannotSelectMissingItem()
        {
            var repository = new Repository<string, TestItem>();
            var selection = new SelectionService<string, TestItem>(repository);

            Assert.IsFalse(selection.TrySelect("missing"));
            Assert.IsFalse(selection.HasSelection);
            Assert.Throws<KeyNotFoundException>(() => selection.Select("missing"));
        }

        [Test]
        public void SelectionClearsWhenSelectedItemIsRemoved()
        {
            var repository = new Repository<string, TestItem>();
            var item = new TestItem("one", "First");
            repository.AddOrUpdate(item);

            var selection = new SelectionService<string, TestItem>(repository);
            selection.Select("one");

            SelectionChangedEventArgs<string, TestItem> args = null;
            selection.SelectionChanged += (_, changedArgs) => args = changedArgs;

            repository.Remove("one");

            Assert.IsFalse(selection.HasSelection);
            Assert.IsNull(selection.SelectedItem);
            Assert.NotNull(args);
            Assert.AreEqual(SelectionChangeMode.RepositoryChanged, args.Mode);
            Assert.IsFalse(args.HasSelection);
            Assert.IsTrue(args.HadPreviousSelection);
            Assert.AreEqual("one", args.PreviousKey);
            Assert.AreSame(item, args.PreviousItem);
        }

        [Test]
        public void SelectionClearsWhenRepositoryIsCleared()
        {
            var repository = new Repository<string, TestItem>();
            repository.AddOrUpdate(new TestItem("one", "First"));

            var selection = new SelectionService<string, TestItem>(repository);
            selection.Select("one");

            repository.Clear();

            Assert.IsFalse(selection.HasSelection);
            Assert.IsNull(selection.SelectedItem);
        }

        [Test]
        public void SelectionRaisesChangeEvent()
        {
            var repository = new Repository<string, TestItem>();
            var item = new TestItem("one", "First");
            repository.AddOrUpdate(item);

            var selection = new SelectionService<string, TestItem>(repository);
            var events = new List<SelectionChangedEventArgs<string, TestItem>>();
            selection.SelectionChanged += (_, args) => events.Add(args);

            selection.Select("one");
            selection.Clear(SelectionChangeMode.Programmatic);

            Assert.AreEqual(2, events.Count);
            Assert.IsTrue(events[0].HasSelection);
            Assert.AreEqual("one", events[0].SelectedKey);
            Assert.AreSame(item, events[0].SelectedItem);
            Assert.AreEqual(SelectionChangeMode.Manual, events[0].Mode);

            Assert.IsFalse(events[1].HasSelection);
            Assert.AreEqual("one", events[1].PreviousKey);
            Assert.AreSame(item, events[1].PreviousItem);
            Assert.AreEqual(SelectionChangeMode.Programmatic, events[1].Mode);
        }

        [Test]
        public void ReplacingRepositoryItemKeepsSelectionByKeyValid()
        {
            var repository = new Repository<string, TestItem>();
            var original = new TestItem("one", "First");
            var replacement = new TestItem("one", "Replacement");
            repository.AddOrUpdate(original);

            var selection = new SelectionService<string, TestItem>(repository);
            selection.Select("one");

            repository.AddOrUpdate(replacement);

            Assert.IsTrue(selection.HasSelection);
            Assert.AreEqual("one", selection.SelectedKey);
            Assert.AreSame(replacement, selection.SelectedItem);
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
