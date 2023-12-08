using System.Collections.Generic;
using UnityEngine;

public class Ltg8SaveSystem : MonoBehaviour
{
    private Dictionary<string, bool> _flags = new Dictionary<string, bool>();
    private Dictionary<string, int> _vars = new Dictionary<string, int>();
    
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
}