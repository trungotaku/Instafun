namespace Constants
{
    public static class AudioEvent
    {
        public const string PLAY_SOUND = "PLAY_SOUND";
        public const string SET_SOUND_VOLUME = "SET_SOUND_VOLUME";
        public const string SET_MUSIC_VOLUME = "SET_MUSIC_VOLUME";
        public const string PAUSE_MUSIC = "PAUSE_MUSIC";
        public const string UN_PAUSE_MUSIC = "UN_PAUSE_MUSIC";
    }

    public enum SoundId
    {
        NONE = -1,
        BTN_TAP = 0,
    }
}