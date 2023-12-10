using System.Collections.Generic;
using poetools.Console.Commands;
using UnityEngine;

public class Ltg8SaveSystem : MonoBehaviour, IConsoleDebugInfo
{
    private readonly Dictionary<string, bool> _flags = new Dictionary<string, bool>();
    private readonly Dictionary<string, int> _vars = new Dictionary<string, int>();
    
    public void SetFlag(string id)
    {
        _flags[id] = true;
    }
    
    public void ResetFlag(string id)
    {
        _flags[id] = false;
    }
    
    public bool GetFlag(string id)
    {
        return _flags.TryGetValue(id, out bool isFlagSet) && isFlagSet;
    }
    
    public void SetVar(string id, int value)
    {
        _vars[id] = value;
    }
    
    public int GetVar(string id)
    {
        return _vars.TryGetValue(id, out int value) ? value : 0;
    }

    public void SaveToDisk(string saveId)
    {
        // todo: impl
    }

    public void LoadFromDisk(string saveId)
    {
        // todo: impl
    }

#region DEBUG

    private string _debugFlagId;
    private string _debugVarId;
    private string _debugVarValue;
    private string _debugSaveId;
    
    public string DebugName => name;
    
    public void DrawDebugInfo()
    {
        if (GUILayout.Button("Save To Disk"))
            SaveToDisk(_debugSaveId);
        
        if (GUILayout.Button("Load From Disk"))
            LoadFromDisk(_debugSaveId);

        if (GUILayout.Button("Open Save Folder"))
        {
            // todo: impl
        }
    }
    
#endregion
    
}