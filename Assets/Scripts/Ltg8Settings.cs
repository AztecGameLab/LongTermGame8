using TriInspector;
using UnityEngine;

namespace Ltg8
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = "LTG8/Settings")]
    public class Ltg8Settings : ScriptableObject
    {
        [Scene]
        public string persistentScenePath;
    }
}