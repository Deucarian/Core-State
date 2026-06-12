using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deucarian.CoreState;
using UnityEngine;

namespace Deucarian.CoreState.Samples.StandaloneRepositorySelection
{
    public sealed class CoreStateStandaloneSample : MonoBehaviour
    {
        private readonly Repository<int, FakeProjectData> _repository = new Repository<int, FakeProjectData>();
        private SelectionService<int, FakeProjectData> _selection;
        private string _lastMessage = "Ready.";

        private void Awake()
        {
            _selection = new SelectionService<int, FakeProjectData>(_repository);
            _selection.SelectionChanged += OnSelectionChanged;
            ResetRepository();
        }

        private void OnDestroy()
        {
            if (_selection != null)
            {
                _selection.SelectionChanged -= OnSelectionChanged;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(24f, 24f, 520f, 560f), GUI.skin.box);
            GUILayout.Label("CoreState Standalone Sample");
            GUILayout.Label("Repository<int, FakeProjectData> + SelectionService<int, FakeProjectData>");
            GUILayout.Space(8f);

            GUILayout.Label(BuildRepositoryText());
            GUILayout.Space(8f);
            GUILayout.Label(BuildSelectionText());
            GUILayout.Label(_lastMessage);
            GUILayout.Space(12f);

            GUILayout.BeginHorizontal();
            DrawSelectButton(1);
            DrawSelectButton(2);
            DrawSelectButton(3);
            GUILayout.EndHorizontal();

            GUILayout.Space(6f);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Try Select Missing Key 99"))
            {
                bool selected = _selection.TrySelect(99);
                _lastMessage = selected ? "Selected 99." : "Key 99 is not in the repository.";
            }

            if (GUILayout.Button("Remove Selected"))
            {
                RemoveSelected();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear Repository"))
            {
                _repository.Clear();
                _lastMessage = "Repository cleared.";
            }

            if (GUILayout.Button("Reset Fake Data"))
            {
                ResetRepository();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawSelectButton(int key)
        {
            bool exists = _repository.ContainsKey(key);
            GUI.enabled = exists;

            if (GUILayout.Button(exists ? $"Select {key}" : $"Missing {key}"))
            {
                _selection.Select(key);
            }

            GUI.enabled = true;
        }

        private void RemoveSelected()
        {
            if (!_selection.HasSelection)
            {
                _lastMessage = "Nothing selected.";
                return;
            }

            int key = _selection.SelectedKey;
            _repository.Remove(key);
            _lastMessage = $"Removed key {key}. Selection clears because the selected repository item is gone.";
        }

        private void ResetRepository()
        {
            _repository.Clear();
            _repository.AddOrUpdateMany(new[]
            {
                new FakeProjectData(1, "Project Alpha"),
                new FakeProjectData(2, "Project Beta"),
                new FakeProjectData(3, "Project Gamma")
            });
            _lastMessage = "Repository reset with fake data.";
        }

        private string BuildRepositoryText()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Repository count: {_repository.Count}");

            foreach (FakeProjectData item in _repository.Items.OrderBy(item => item.Id))
            {
                builder.AppendLine($"- {item.Id}: {item.Name}");
            }

            return builder.ToString();
        }

        private string BuildSelectionText()
        {
            if (!_selection.HasSelection)
            {
                return "Selection: none";
            }

            FakeProjectData selected = _selection.SelectedItem;
            return selected != null
                ? $"Selection: key {_selection.SelectedKey}, item {selected.Name}"
                : $"Selection: key {_selection.SelectedKey}, item missing";
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs<int, FakeProjectData> args)
        {
            _lastMessage = args.HasSelection
                ? $"Selection changed to key {args.SelectedKey}."
                : "Selection cleared.";
        }
    }
}
