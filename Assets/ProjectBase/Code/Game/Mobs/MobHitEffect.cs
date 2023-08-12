using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Mobs
{
    public class MobHitEffect : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color hitColor = Color.red;

        private CancellationTokenSource _cancellationTokenSource;
        
        private static readonly int ShaderColor = Shader.PropertyToID("_Color");

        public async void PlayHitEffect(float duration)
        {
            CancelEffectIfThereIs();
            
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
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

        public void CancelEffectIfThereIs()
        {
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