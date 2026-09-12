using System;
using UnityEngine;

namespace Deucarian.CoreState.Samples.DefinitionWorkflow
{
    /// <summary>Small caller example. The configured scene hosts own services and resource lifetimes.</summary>
    public sealed class CoreStateWorkflow : MonoBehaviour
    {
        private readonly Repository<Guid, SampleItem> items = new Repository<Guid, SampleItem>();
        private SampleItem current;
        private string status = "Ready. Choose an action below.";
        public string Status => status;
        public void Add() { current = new SampleItem(); items.AddOrUpdate(current); status = "Items: " + items.Count; }
        public void Remove() { if (current != null) items.Remove(current.Id); current = null; status = "Items: " + items.Count; }
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(24, 24, Math.Min(540, Screen.width - 48), Screen.height - 48), GUI.skin.box);
            GUILayout.Label("Core-State — definition workflow");
            GUILayout.Label("Repository identities belong to actual runtime items. The pure C# repository works from this component without generating global keys or content assets.");
            GUILayout.Space(12);
            if (GUILayout.Button("Add typed item", GUILayout.Height(32))) { try { Add(); } catch (Exception error) { status = error.Message; } }
            if (GUILayout.Button("Remove current item", GUILayout.Height(32))) { try { Remove(); } catch (Exception error) { status = error.Message; } }
            GUILayout.Space(12);
            GUILayout.Label(status);
            GUILayout.EndArea();
        }
    }
}
