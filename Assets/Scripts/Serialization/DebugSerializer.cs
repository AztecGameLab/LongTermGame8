using TNRD;
using TriInspector;
using UnityEngine;

namespace Serialization
{
    [HideMonoScript]
    public class DebugSerializer : MonoBehaviour, ISerializer
    {
        [SerializeField] 
        private SerializableInterface<ISerializer> serializer;

        private static void LogMissingSerializerWarning()
        {
            Debug.LogWarning("SERIALIZER: No serializer is set, so only logging will take place.");
        }
        
        [Title("Play-mode Controls")]

        [Button, EnableInPlayMode]
        public void SetFlag(string id)
        {
            Debug.Log($"SERIALIZER: Setting flag {id}");

            if (serializer.TryGetValue(out ISerializer s))
                s.SetFlag(id);
        
            else LogMissingSerializerWarning();
        }
    
        [Button, EnableInPlayMode]
        public void ResetFlag(string id)
        {
            Debug.Log($"SERIALIZER: Resetting flag {id}");
        
            if (serializer.TryGetValue(out ISerializer s))
                s.ResetFlag(id);
        
            else LogMissingSerializerWarning();
        }
    
        [Button, EnableInPlayMode]
        public bool GetFlag(string id)
        {
            Debug.Log($"SERIALIZER: Getting flag {id}");
        
            if (serializer.TryGetValue(out ISerializer s))
                return s.GetFlag(id);
        
            LogMissingSerializerWarning();
            return false;
        }
    
        [Button, EnableInPlayMode]
        public void SetVar(string id, int value)
        {
            Debug.Log($"SERIALIZER: Setting var {id} to {value}");

            if (serializer.TryGetValue(out ISerializer s))
                s.SetVar(id, value);
        
            else LogMissingSerializerWarning();
        }
    
        [Button, EnableInPlayMode]
        public int GetVar(string id)
        {
            Debug.Log($"SERIALIZER: Getting var {id}");
        
            if (serializer.TryGetValue(out ISerializer s))
                return s.GetVar(id);
        
            LogMissingSerializerWarning();
            return 0;
        }
    }
}
