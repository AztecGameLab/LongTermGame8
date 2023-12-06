namespace Serialization
{
    public interface ISerializer
    {
        public void SetFlag(string id) ;
        public void ResetFlag(string id);
        public bool GetFlag(string id);
    
        public void SetVar(string id, int value);
        public int GetVar(string id);
    }
}