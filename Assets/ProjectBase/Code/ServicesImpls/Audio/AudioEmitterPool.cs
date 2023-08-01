﻿using Zenject;

namespace ServicesImpls.Audio
{
    public class AudioEmitterPool : MonoMemoryPool<AudioEmitter>
    {
        protected override void Reinitialize(AudioEmitter emitter)
        {
            emitter.ResetEmitter();
        }
    }
}