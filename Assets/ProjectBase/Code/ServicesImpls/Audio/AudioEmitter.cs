using UnityEngine;

namespace ServicesImpls.Audio
{
    public class AudioEmitter : MonoBehaviour
    {
        [SerializeField] private AudioSource unityAudioSource;

        public float InactiveTime
        {
            get;
            private set;
        }

        public AudioClip Clip
        {
            get => unityAudioSource.clip;
            set => unityAudioSource.clip = value;
        }

        public float Volume
        {
            get => unityAudioSource.volume;
            set => unityAudioSource.volume = value;
        }
        
        public float Pitch
        {
            get => unityAudioSource.pitch;
            set => unityAudioSource.pitch = value;
        }
        
        public bool Loop
        {
            get => unityAudioSource.loop;
            set => unityAudioSource.loop = value;
        }

        private void Update()
        {
            if (!unityAudioSource.isPlaying)
            {
                InactiveTime += Time.deltaTime;
            }
        }

        public void Play()
        {
            unityAudioSource.Play();
        }
        
        public void Stop()
        {
            unityAudioSource.Stop();
        }

        public void ResetEmitter()
        {
            ResetInactiveTime();
            ResetUnityAudioSource();
        }

        private void ResetInactiveTime()
        {
            InactiveTime = 0f;
        }

        private void ResetUnityAudioSource()
        {
            unityAudioSource.playOnAwake = false;
            unityAudioSource.clip = null;
            unityAudioSource.loop = false;
            unityAudioSource.mute = false;
            unityAudioSource.pitch = 1f;
            unityAudioSource.priority = 128;
            unityAudioSource.volume = 1f;
        }
    }
}