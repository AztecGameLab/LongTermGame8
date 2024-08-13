using Cysharp.Threading.Tasks;

namespace Ltg8
{
    /// <summary>
    ///   <b>Description</b>: Interface for the SaveSerializer classes
    /// </summary>
    public interface ISaveSerializer
    {
        // Method used to write data to the disk
        UniTask WriteToDisk(string saveId, SaveData data);
        // Method used to read and grab data from the disk
        UniTask<SaveData> ReadFromDisk(string saveId);
    }
}