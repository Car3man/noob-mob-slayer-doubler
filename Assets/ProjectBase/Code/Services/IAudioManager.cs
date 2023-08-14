namespace Services
{
    public interface IAudioManager
    {
        bool Mute { get; set; }
        bool MusicMute { get; set; }
        bool SoundMute { get; set; }
        float MusicVolume { get; set; }
        float SoundVolume { get; set; }
        event MuteChangeDelegate OnMuteChange;
        event MusicMuteChangeDelegate OnMusicMuteChange;
        event SoundMuteChangeDelegate OnSoundMuteChange;
        event MusicVolumeChangeDelegate OnMusicVolumeChange;
        event SoundVolumeChangeDelegate OnSoundVolumeChange;
        void PlayMusicIfNotSame(string name, float volume = 1f);
        void PlayMusic(string name, float volume);
        void PlaySound(string name, bool loop = false, float volume = 1f, float pitch = 1f);
        void StopMusic();
        void StopSound(string name);
    }

    public delegate void MuteChangeDelegate(bool val);
    public delegate void MusicMuteChangeDelegate(bool val);
    public delegate void SoundMuteChangeDelegate(bool val);
    public delegate void MusicVolumeChangeDelegate(float val);
    public delegate void SoundVolumeChangeDelegate(float val);
}