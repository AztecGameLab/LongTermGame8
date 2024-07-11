using TriInspector;
using UnityEngine;

namespace Ltg8
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = "LTG8/DialogueSettings")]
    public class Ltg8DialogueSettings : ScriptableObject
    {
        public int selected = 0;
        
        public float[] dialogueSpeedMultipliers =
        {
            0.75f,
            1f,
            1.25f
        };
    }
}