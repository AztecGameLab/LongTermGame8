using System.IO;
using Cysharp.Threading.Tasks;
using poetools.Console.Commands;
using UnityEngine;

public class Ltg8SaveSerializer : MonoBehaviour, IConsoleDebugInfo
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
        Ltg8.Save = JsonUtility.FromJson<Ltg8SaveData>(json);
        Busy = false;
    }

#region DEBUG

    private string _debugFlagId;
    private string _debugVarId;
    private string _debugVarValue;
    private string _debugSaveId = "dev_test.json";
    
    public string DebugName => name;
    
    public void DrawDebugInfo()
    {
        if (!Busy && GUILayout.Button("Save To Disk"))
            WriteToDisk(_debugSaveId).Forget();
        
        if (!Busy && GUILayout.Button("Load From Disk"))
            ReadFromDisk(_debugSaveId).Forget();

#if UNITY_EDITOR
        if (GUILayout.Button("Open Save Folder"))
            UnityEditor.EditorUtility.RevealInFinder(SavePath);
#endif
    }
    
#endregion
    
}