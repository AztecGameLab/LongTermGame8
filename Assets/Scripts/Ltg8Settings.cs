using TriInspector;
using UnityEngine;

[HideMonoScript]
[CreateAssetMenu(menuName = "LTG8/Settings")]
public class Ltg8Settings : ScriptableObject
{
    [Scene]
    public string persistentScenePath;
}