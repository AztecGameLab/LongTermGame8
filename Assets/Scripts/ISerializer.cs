public interface ISerializer
{
    public T GetValue<T>(string id);
    public void SetValue<T>(string id, T value);
}

// Some common serialization cases
public static class SerializerExtensions
{
    public static void SetFlag(this ISerializer serializer, string id)  => serializer.SetValue(id, true);
    public static void ResetFlag(this ISerializer serializer, string id) => serializer.SetValue(id, false);
    public static bool GetFlag(this ISerializer serializer, string id) => serializer.GetValue<bool>(id);
    
    public static void SetVar(this ISerializer serializer, string id, int value) => serializer.SetValue(id, value);
    public static int GetVar(this ISerializer serializer, string id) => serializer.GetValue<int>(id);
}
