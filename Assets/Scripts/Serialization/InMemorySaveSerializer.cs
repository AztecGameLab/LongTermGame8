using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Ltg8
{
    public class InMemorySaveSerializer : MonoBehaviour, ISaveSerializer
    {
        private readonly Dictionary<string, SaveData> _saves = new Dictionary<string, SaveData>();
        
        public UniTask WriteToDisk(string saveId, SaveData data)
        {
            if (!_saves.TryAdd(saveId, data))
                _saves[saveId] = data;

            return UniTask.CompletedTask;
        }
        
        public UniTask<SaveData> ReadFromDisk(string saveId)
        {
            _saves.TryAdd(saveId, new SaveData());
            return new UniTask<SaveData>(_saves[saveId]);
        }
    }
}
