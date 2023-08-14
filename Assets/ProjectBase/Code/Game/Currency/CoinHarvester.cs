using System.Collections.Generic;
using Services;
using UnityEngine;
using Zenject;

namespace Game.Currency
{
    public class CoinHarvester : ITickable, IFixedTickable
    {
        private readonly Camera _camera;
        private readonly IAudioManager _audioManager;
        private readonly CoinSpawner _coinSpawner;
        private readonly CurrencyController _currencyController;
        private readonly RaycastHit[] _hits = new RaycastHit[3];
        private readonly List<Coin> _coinsToAutoCollect = new();
        
        private const float AutoCollectTimeOut = 10f;

        public CoinHarvester(
            Camera camera,
            IAudioManager audioManager,
            CoinSpawner coinSpawner,
            CurrencyController currencyController
            )
        {
            _camera = camera;
            _audioManager = audioManager;
            _coinSpawner = coinSpawner;
            _currencyController = currencyController;
        }

        public void Tick()
        {
            foreach (var coin in _coinSpawner.Coins)
            {
                if (Time.time - _coinSpawner.GetSpawnTime(coin) >= AutoCollectTimeOut)
                {
                    _coinsToAutoCollect.Add(coin);
                }
            }
            
            foreach (var coinToCollect in _coinsToAutoCollect)
            {
                CollectCoin(coinToCollect);
            }
            
            _coinsToAutoCollect.Clear();
        }

        public void FixedTick()
        {
            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePosition);
            var countHits =
                Physics.RaycastNonAlloc(ray, _hits, float.MaxValue, LayerMask.GetMask("Coin"));
            for (int i = 0; i < countHits; i++)
            {
                var hitTransform = _hits[i].transform;
                var coin = hitTransform.GetComponent<Coin>();
                
                if (coin.WasTook)
                {
                    continue;
                }

                if (_coinSpawner.IsCoinDropAnimationPlaying(coin))
                {
                    return;
                }
                
                CollectCoin(coin);
            }
        }

        public void CollectCoin(Coin coin)
        {
            coin.MarkWasTook();
            _currencyController.AddCoins(coin.Value);
            _coinSpawner.Despawn(coin);
            _audioManager.PlaySound("Coin", volume: 0.33f);
        }
    }
}
