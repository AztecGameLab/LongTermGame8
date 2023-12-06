public interface IMusicPlayer
{
    public struct MusicHandle {}
    
    public MusicHandle PushMusic();
    public void StopMusic(MusicHandle handle);
    public void StopAllMusic();
}
