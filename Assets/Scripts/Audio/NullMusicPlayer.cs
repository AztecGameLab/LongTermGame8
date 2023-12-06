namespace Audio
{
    public class NullMusicPlayer : IMusicPlayer
    {
        public static readonly NullMusicPlayer Instance = new NullMusicPlayer();
    }
}
