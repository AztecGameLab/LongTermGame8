namespace Serialization
{
    public class NullSerializer : ISerializer
    {
        public static readonly NullSerializer Instance = new NullSerializer();
        
        public void SetFlag(string id) {}
        public void ResetFlag(string id) {}
        public bool GetFlag(string id) => false;
        
        public void SetVar(string id, int value) {}
        public int GetVar(string id) => 0;
    }
}
