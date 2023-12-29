using Cysharp.Threading.Tasks;

namespace Ltg8
{
    public interface ISaveSerializer
    {
        UniTask WriteToDisk(string saveId, SaveData data);
        UniTask<SaveData> ReadFromDisk(string saveId);
    }
}