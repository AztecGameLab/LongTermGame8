using System.IO;
using Cysharp.Threading.Tasks;
using poetools.Console.Commands;
using UnityEngine;
namespace Ltg8
{
    /*
     * Description
     * This class is responsible for the methods that read, write, and organize the game's Save Data files
     */
    public class DiskSaveSerializer : MonoBehaviour, IConsoleDebugInfo, ISaveSerializer
    {
        
        // This creates a string that hols the path to the games "saves" folders
        // NOTE: => is a shorthand way of executing the function to the right and storing the returned value to the variable on the left
        // NOTE: The $ symbol allows for interpolated strings. Basically, whatever is inside the {} is evaluated & then added onto the string 
        // NOTE: Application.persistentDataPath is a path that leads to the folder where data is saved & preserved between runs
        public static string SavePath => $"{Application.persistentDataPath}/saves";
        // A boolean to determine whether the program is currently executing one of these methods
        public bool Busy { get; private set; } 
    
        // NOTE: Async allows the usage of Tasks and the Await function
        // This method writes the saved data into the .json file specified by the saveId
        public async UniTask WriteToDisk(string saveId, SaveData data)
        {
            if (Busy) return; // If a method is currently running, do not run this method
        
            Busy = true; // This Method is now running
            // NOTE: ToJson is used to create a JSON representation of the public fields of an object (Pretty print adds readability)
            // The unused variables PlayerPos, PlayerSceneID, along w/ Inventory are made into a JSON representation & stored in json
            string json = JsonUtility.ToJson(data, true);
            // The directory to the .json file of the specified saveID
            string path = $"{SavePath}/{saveId}.json";
            // If the path specified does not exist, create it
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(path);
            // Write the texts stored in the json string into the .json file at the specified directory (path)
            await File.WriteAllTextAsync(path, json);
            Busy = false; // This Method is no longer running
        }

        // This method reads the data from the .json file specified by the saveId, and stores it in a new SaveData object
        public async UniTask<SaveData> ReadFromDisk(string saveId)
        {
            if (Busy) return null; // If a method is currently running, do not run this one
            Busy = true; 
            
            // The directory to the .json file of the specified saveID
            string path = $"{SavePath}/{saveId}.json";

            // Not sure if this is best approach for unavailable saves, but it's good for now - Daniel
            if (!File.Exists(path))
                return new SaveData();
            
            // Read the texts in the .json file at the specified directory (path) and store it in the json string
            string json = await File.ReadAllTextAsync(path);
            
            // Using the json string, fill the fields in a new SaveData object
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            // The method is no longer running
            Busy = false;
            
            return data; // Return the new data object
        }

        public string DebugName => name; // Set string DebugName to the name of the object
        public const string DebugSaveId = "dev_test"; 
        private string _currentDebugSaveId = DebugSaveId;
    
        // NOTE: Return to this later
        // Not exactly sure, but it seems like this method allows a developer to read or modify Save Data files via buttons & text boxes
        public void DrawDebugInfo()
        {
            GUILayout.Label("Target Save ID"); // Creates a text GUI with the given string
            _currentDebugSaveId = GUILayout.TextField(_currentDebugSaveId); // Allows the _currentDebugSaveID to be modified
            GUI.enabled = !Busy; // If one of the methods is running, enable the GUI, otherwise, leave it disabled

            // If the created Button "Save To Disk" is pressed
            if (GUILayout.Button("Save To Disk"))
            {
                // Set the DevTest Flag to "true", and set its var to 5
                Ltg8.Save.SetFlag(Flag.DevTest);
                Ltg8.Save.SetVar(Var.DevTest, 5);
                
                // Write the Save Data to the .json file specified by the _CurrentDebugSaveID 
                WriteToDisk(_currentDebugSaveId, Ltg8.Save).Forget();
            }
        
            // If the "Load From Disk" button is pressed
            if (GUILayout.Button("Load From Disk"))
                // Set the Save Data to the data in the .json file specified by _CurrentDebugSaveId
                Ltg8.Save = ReadFromDisk(_currentDebugSaveId).GetAwaiter().GetResult();

            GUI.enabled = true; // Enable GUI

#if UNITY_EDITOR // If in the Unity Editor
            if (GUILayout.Button("Open Save Folder")) // If the Button "Open Save Folder" is pressed
                UnityEditor.EditorUtility.RevealInFinder(SavePath); // Reveal the Save Path directory in Finder
#endif
        }
    }
}
