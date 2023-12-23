using System.IO;
using Cysharp.Threading.Tasks;
using poetools.Console.Commands;
using UnityEngine;

namespace Ltg8
{
    public class SaveSerializer : MonoBehaviour, IConsoleDebugInfo
    {
        public static string SavePath => $"{Application.persistentDataPath}/saves";
        public bool Busy { get; private set; }
    
        public async UniTaskVoid WriteToDisk(string saveId)
        {
            if (Busy) return;
        
            Busy = true;
            string json = JsonUtility.ToJson(Ltg8.Save, true);
            string path = $"{SavePath}/{saveId}";
        
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(path);
        
            await File.WriteAllTextAsync(path, json);
            Busy = false;
        }

        public async UniTaskVoid ReadFromDisk(string saveId)
        {
            if (Busy) return;
        
            Busy = true;
            string json = await File.ReadAllTextAsync($"{SavePath}/{saveId}");
            Ltg8.Save = JsonUtility.FromJson<SaveData>(json);
            Busy = false;
        }

        public string DebugName => name;
        private string _debugSaveId = "dev_test.json";
    
        public void DrawDebugInfo()
        {
            GUILayout.Label("Target Save ID");
            _debugSaveId = GUILayout.TextField(_debugSaveId);
            GUI.enabled = !Busy;

            if (GUILayout.Button("Save To Disk"))
            {
                Ltg8.Save.SetFlag(Flag.DevTest);
                Ltg8.Save.SetVar(Var.DevTest, 5);
                WriteToDisk(_debugSaveId).Forget();
            }
        
            if (GUILayout.Button("Load From Disk"))
                ReadFromDisk(_debugSaveId).Forget();

            GUI.enabled = true;

#if UNITY_EDITOR
            if (GUILayout.Button("Open Save Folder"))
                UnityEditor.EditorUtility.RevealInFinder(SavePath);
#endif
        }
    }
}