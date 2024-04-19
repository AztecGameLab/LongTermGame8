using Ltg8.Inventory;
using TriInspector;
using UnityEngine;

namespace Ltg8
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = "LTG8/Settings")]
    public class Ltg8Settings : ScriptableObject
    {
        [Scene] public string persistentScenePath;
        [Scene] public string mainMenuScenePath;
        [Scene] public string defaultOverworldScenePath;
        public InventoryItemWorldDisplay overworldItemPrefab;

        [Title("Editor")]

        public EditorPlayStrategy editorPlayStrategy = EditorPlayStrategy.FromCurrentScene;
        public EditorSaveStrategy editorSaveStrategy = EditorSaveStrategy.FromDisk;
        public string editorSaveId = DiskSaveSerializer.DebugSaveId;
    }
    
    public enum EditorPlayStrategy
    {
        FromCurrentScene,
        FromStartOfGame,
    }

    public enum EditorSaveStrategy
    {
        NonPersistent,
        FromDisk,
    }
}