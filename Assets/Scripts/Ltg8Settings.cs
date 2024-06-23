using Inventory;
using Ltg8.Inventory;
using TriInspector;
using UnityEngine;

namespace Ltg8
{
    [HideMonoScript] // NOTE: Prevents something from showing up in the inspector
    [CreateAssetMenu(menuName = "LTG8/Settings")]
    public class Ltg8Settings : ScriptableObject
    {
        // NOTE: [Scene] allows having scenes as an input in the inspector
        [Scene] public string persistentScenePath; // Input for the persistent scene
        [Scene] public string mainMenuScenePath; // Input for the main menu scene
        public InventoryItemWorldDisplay overworldItemPrefab; //  

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