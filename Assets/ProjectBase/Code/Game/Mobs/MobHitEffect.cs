using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Mobs
{
    public class MobHitEffect : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color hitColor = Color.red;

        private IAudioManager _audioManager;
        private CancellationTokenSource _cancellationTokenSource;

        private static readonly int ShaderColor = Shader.PropertyToID("_Color");

        [Zenject.Inject]
        public void Construct(IAudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        public void PlayHitEffect(float duration)
        {
            CancelEffectIfThereIs();
            _cancellationTokenSource = new CancellationTokenSource();
            
            PlayColorEffect(duration, _cancellationTokenSource.Token);
            PlayScaleTween(duration);
            PlaySoundEffect();
        }

        private void PlayScaleTween(float duration)
        {
            var scaleTweenSeq = DOTween.Sequence(transform);
            scaleTweenSeq.Append(
                transform
                    .DOScale(0.9f, duration / 2f)
            );
            scaleTweenSeq.Append(
                transform
                    .DOScale(1f, duration / 2f)
            );
        }

        private async void PlayColorEffect(float duration, CancellationToken cancellationToken)
        {
            SetColorForRenderers(hitColor);
            try
            {
                await UniTask.WaitForSeconds(duration, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            SetColorForRenderers(normalColor);
        }

        private void PlaySoundEffect()
        {
            var rndHitClip = Random.Range(1, 4);
            _audioManager.PlaySound($"Hit{rndHitClip}", false, 1f, 1f);
        }

        public void CancelEffectIfThereIs()
        {
            transform.DOKill(true);
            
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }
        }

        private void SetColorForRenderers(Color color)
        {
            foreach (var mobRenderer in renderers)
            {
                foreach (var material in mobRenderer.materials)
                {
                    material.SetColor(ShaderColor, color);
                }
            }
        }
    }
}