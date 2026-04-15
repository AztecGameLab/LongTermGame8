using Inventory;
using TriInspector;
using UnityEngine;

namespace Ltg8
{
    
    /// <summary>
    /// <b>Description</b>: Responsible for the game's settings; specifically those relating to the game's
    /// <list type="number">
    ///       <item> <description><b>Starting Protocol</b> - Which scene it starts from </description> </item>
    ///       <item> <description><b>Save/Load Protocol</b> - Whether it's non-persistent or uses the disk </description> </item>
    ///       <item> <description><b>Critical scenes</b> - Persistent / Main Menu </description> </item>
    ///   </list>
    /// </summary>
    [HideMonoScript] // NOTE: Hides the "Script: [     ]" component in the Insepector
    [CreateAssetMenu(menuName = "LTG8/Settings")] 
    public class Ltg8Settings : ScriptableObject
    {

        [Title("Scenes")]
        
        // NOTE: [Scene] allows having scenes as an input in the inspector
        [Scene] public string persistentScenePath; // The path of the persistent scene (persists throughout the game's runtime)
        [Scene] public string mainMenuScenePath; // The path of the main menu scene
        
        [Header("Scene Data")]
        public InventoryItemWorldDisplay overworldItemPrefab; 
        
        [Title("Editor")]

        // The scene to start from when playing in the Editor
        public EditorPlayStrategy editorPlayStrategy = EditorPlayStrategy.FromCurrentScene;
        // Sets the EditorSaveStrategy to "FromDisk" upon creation of the ScriptableObject
        public EditorSaveStrategy editorSaveStrategy = EditorSaveStrategy.FromDisk;
        // ID for the editor save file?
        public string editorSaveId = DiskSaveSerializer.DebugSaveId;
    }
    
    /// <summary>
    ///   <b>Description</b>: When you press "Play" in the inspector, this will determine where the game starts from
    ///   <para>(Either the current scene you're modifying, or the beginning of the game)</para>
    /// </summary>
    public enum EditorPlayStrategy 
    {
        FromCurrentScene,
        FromStartOfGame,
    }
    
    /// <summary>
    ///   <b>Description</b>: This determines whether data is non-persistent between runs or whether it is saved to and read from the disk (I think)
    /// </summary>
    public enum EditorSaveStrategy
    {
        NonPersistent,
        FromDisk,
    }
}