using FMOD.Studio;
namespace DefaultNamespace
{
    public static class FmodExtensions
    {
        public static string GetPath(this EventInstance instance)
        {
            instance.getDescription(out EventDescription desc);
            desc.getPath(out string path);
            return path;
        }
    }
}
