using System.Linq;
using Game.Currency;
using Infrastructure.SceneStarters;
using Zenject;

namespace Game.AutoPlayTest
{
    public class AutoCoinHarvester : ITickable
    {
        private readonly GameSceneStarter _gameSceneStarter;
        private readonly CoinSpawner _coinSpawner;
        private readonly CoinHarvester _coinHarvester;
        
        public AutoCoinHarvester(
            GameSceneStarter gameSceneStarter,
            CoinSpawner coinSpawner,
            CoinHarvester coinHarvester
            )
        {
            _gameSceneStarter = gameSceneStarter;
            _coinSpawner = coinSpawner;
            _coinHarvester = coinHarvester;
        }

        public void Tick()
        {
            if (!_gameSceneStarter.Started)
            {
                return;
            }
            
            var coinsToCollect = _coinSpawner.Coins
                .Where(coin => !_coinSpawner.IsCoinDropAnimationPlaying(coin))
                .ToList();
            foreach (var coin in coinsToCollect)
            {
                _coinHarvester.CollectCoin(coin);
            }
        }
    }
}