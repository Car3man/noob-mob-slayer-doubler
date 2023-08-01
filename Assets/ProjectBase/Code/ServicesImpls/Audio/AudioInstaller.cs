using Services;
using Zenject;

namespace ServicesImpls.Audio
{
    public class AudioInstaller : Installer<AudioInstaller>
    {
        private readonly string _audioEmitterPrefabPath;

        public AudioInstaller(string audioEmitterPrefabPath = "AudioEmitter")
        {
            _audioEmitterPrefabPath = audioEmitterPrefabPath;
        }
        
        public override void InstallBindings()
        {
            Container.Bind<AudioClipProvider>().AsSingle();
            Container.BindMemoryPool<AudioEmitter, AudioEmitterPool>()
                .FromComponentInNewPrefabResource(_audioEmitterPrefabPath);
            Container.Bind(typeof(IAudioManager), typeof(ITickable)).To<AudioManager>().AsSingle();
        }
    }
}