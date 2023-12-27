using System.IO;
using Cysharp.Threading.Tasks;
using poetools.Console.Commands;
using UnityEngine;
namespace Ltg8
{
    public class DiskSaveSerializer : MonoBehaviour, IConsoleDebugInfo, ISaveSerializer
    {
        public static string SavePath => $"{Application.persistentDataPath}/saves";
        public bool Busy { get; private set; }
    
        public async UniTask WriteToDisk(string saveId, SaveData data)
        {
            if (Busy) return;
        
            Busy = true;
            string json = JsonUtility.ToJson(data, true);
            string path = $"{SavePath}/{saveId}.json";
        
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(path);
        
            await File.WriteAllTextAsync(path, json);
            Busy = false;
        }

        public async UniTask<SaveData> ReadFromDisk(string saveId)
        {
            if (Busy) return null;
        
            Busy = true;
            string json = await File.ReadAllTextAsync($"{SavePath}/{saveId}.json");
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Busy = false;
            return data;
        }

        public string DebugName => name;
        public const string DebugSaveId = "dev_test";
        private string _currentDebugSaveId = DebugSaveId;
    
        public void DrawDebugInfo()
        {
            GUILayout.Label("Target Save ID");
            _currentDebugSaveId = GUILayout.TextField(_currentDebugSaveId);
            GUI.enabled = !Busy;

            if (GUILayout.Button("Save To Disk"))
            {
                Ltg8.Save.SetFlag(Flag.DevTest);
                Ltg8.Save.SetVar(Var.DevTest, 5);
                WriteToDisk(_currentDebugSaveId, Ltg8.Save).Forget();
            }
        
            if (GUILayout.Button("Load From Disk"))
                Ltg8.Save = ReadFromDisk(_currentDebugSaveId).GetAwaiter().GetResult();

            GUI.enabled = true;

#if UNITY_EDITOR
            if (GUILayout.Button("Open Save Folder"))
                UnityEditor.EditorUtility.RevealInFinder(SavePath);
#endif
        }
    }
}
