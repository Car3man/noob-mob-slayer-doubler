using System.Collections.Generic;
using Services;
using UnityEngine;
using Zenject;

namespace ServicesImpls.Audio
{
    public class AudioManager : IAudioManager, ITickable
    {
        private readonly AudioClipProvider _clipProvider;
        private readonly AudioEmitterPool _emittersPool;
        private readonly List<AudioEmitter> _musicEmitters = new(1);
        private readonly List<AudioEmitter> _soundEmitters = new(32);
        private readonly Dictionary<AudioEmitter, float> _baseEmittersVolume = new();

        private bool _mute;
        private bool _musicMute;
        private bool _soundMute;
        private float _musicVolume = 1f;
        private float _soundVolume = 1f;

        public bool Mute
        {
            get => _mute;
            set
            {
                _mute = value;
                OnMuteUpdate();
                OnMuteChange?.Invoke(_mute);
            }
        }
        
        public bool MusicMute
        {
            get => _musicMute;
            set
            {
                _musicMute = value;
                OnMuteUpdate();
                OnMusicMuteChange?.Invoke(_musicMute);
            }
        }
        
        public bool SoundMute
        {
            get => _soundMute;
            set
            {
                _soundMute = value;
                OnMuteUpdate();
                OnSoundMuteChange?.Invoke(_soundMute);
            }
        }
        
        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = value;
                OnVolumeUpdate();
                OnMusicVolumeChange?.Invoke(_musicVolume);
            }
        }
        
        public float SoundVolume
        {
            get => _soundVolume;
            set
            {
                _soundVolume = value;
                OnVolumeUpdate();
                OnSoundVolumeChange?.Invoke(_soundVolume);
            }
        }
        
        public event MuteChangeDelegate OnMuteChange;
        public event MusicMuteChangeDelegate OnMusicMuteChange;
        public event SoundMuteChangeDelegate OnSoundMuteChange;
        public event MusicVolumeChangeDelegate OnMusicVolumeChange;
        public event SoundVolumeChangeDelegate OnSoundVolumeChange;

        public AudioManager(
            AudioClipProvider clipProvider,
            AudioEmitterPool emittersPool
            )
        {
            _clipProvider = clipProvider;
            _emittersPool = emittersPool;
        }
        
        public void Tick()
        {
            var musicAudioEmittersToRemove = new List<AudioEmitter>();
            for (int i = 0; i < _musicEmitters.Count; i++)
            {
                if (_musicEmitters[i].InactiveTime > 1f)
                {
                    _emittersPool.Despawn(_musicEmitters[i]);
                    musicAudioEmittersToRemove.Add(_musicEmitters[i]);
                }
            }
            for (var i = 0; i < musicAudioEmittersToRemove.Count; i++)
            {
                AudioEmitter emitter = musicAudioEmittersToRemove[i];
                _musicEmitters.Remove(emitter);
                _baseEmittersVolume.Remove(emitter);
            }

            var soundAudioEmittersToRemove = new List<AudioEmitter>();
            for (int i = 0; i < _soundEmitters.Count; i++)
            {
                if (_soundEmitters[i].InactiveTime > 1f)
                {
                    _emittersPool.Despawn(_soundEmitters[i]);
                    soundAudioEmittersToRemove.Add(_soundEmitters[i]);
                }
            }
            for (var i = 0; i < soundAudioEmittersToRemove.Count; i++)
            {
                AudioEmitter emitter = soundAudioEmittersToRemove[i];
                _soundEmitters.Remove(emitter);
                _baseEmittersVolume.Remove(emitter);
            }
        }

        public async void PlayMusicIfNotSame(string name, float volume = 1f)
        {
            if (_musicEmitters.Count > 0)
            {
                AudioClip audioResource = await _clipProvider.Get(name, "Music");
                if (_musicEmitters[0].Clip == audioResource)
                {
                    return;
                }
            }
            
            PlayMusic(name, volume);
        }

        public async void PlayMusic(string name, float volume = 1f)
        {
            for (int i = 0; i < _musicEmitters.Count; i++)
            {
                _musicEmitters[i].Stop();
            }

            AudioEmitter instance = _emittersPool.Spawn();
            instance.Loop = true;
            instance.Volume = GetFinalMusicVolume(volume);
            instance.Clip = await _clipProvider.Get(name, "Music");
            instance.Play();
            
            _musicEmitters.Add(instance);
            _baseEmittersVolume.Add(instance, volume);
        }

        public async void PlaySound(string name, bool loop, float volume = 1f, float pitch = 1f)
        {
            AudioEmitter instance = _emittersPool.Spawn();
            instance.Clip = await _clipProvider.Get(name, "Sounds");
            instance.Loop = loop;
            instance.Volume = GetFinalSoundVolume(volume);
            instance.Pitch = pitch;
            instance.Play();
            
            _soundEmitters.Add(instance);
            _baseEmittersVolume.Add(instance, volume);
        }
        
        public void StopMusic()
        {
            for (int i = 0; i < _musicEmitters.Count; i++)
            {
                _musicEmitters[i].Stop();
            }
        }
        
        public async void StopSound(string name)
        {
            AudioClip audioClip = await _clipProvider.Get(name, "Sounds");
            for (int i = 0; i < _soundEmitters.Count; i++)
            {
                if (_soundEmitters[i].Clip == audioClip)
                {
                    _soundEmitters[i].Stop();
                }
            }
        }

        private void OnMuteUpdate()
        {
            UpdateVolumes();
        }

        private void OnVolumeUpdate()
        {
            UpdateVolumes();
        }

        private void UpdateVolumes()
        {
            for (var i = 0; i < _musicEmitters.Count; i++)
            {
                AudioEmitter musicEmitter = _musicEmitters[i];
                float baseVolume = _baseEmittersVolume[musicEmitter];
                musicEmitter.Volume = GetFinalMusicVolume(baseVolume);
            }

            for (var i = 0; i < _soundEmitters.Count; i++)
            {
                AudioEmitter soundEmitter = _soundEmitters[i];
                float baseVolume = _baseEmittersVolume[soundEmitter];
                soundEmitter.Volume = GetFinalSoundVolume(baseVolume);
            }
        }

        private float GetFinalMusicVolume(float volume)
        {
            if (Mute)
            {
                return 0f;
            }
            if (MusicMute)
            {
                return 0f;
            }
            return MusicVolume * volume;
        }
        
        private float GetFinalSoundVolume(float volume)
        {
            if (Mute)
            {
                return 0f;
            }
            if (SoundMute)
            {
                return 0f;
            }
            return SoundVolume * volume;
        }
    }
}
