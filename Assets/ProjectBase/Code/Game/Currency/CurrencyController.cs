using System.Numerics;
using Game.Saves;

namespace Game.Currency
{
    public class CurrencyController
    {
        private readonly GameSaves _gameSaves;
        private BigInteger _coins;
        
        private const int StartCoins = 0;

        public delegate void CoinsChangeDelegate(BigInteger coins);
        public event CoinsChangeDelegate OnCoinsChange;
        
        public CurrencyController(GameSaves gameSaves)
        {
            _gameSaves = gameSaves;
        }

        public void RestoreFromSave()
        {
            _coins = _gameSaves.GetCoins(StartCoins);
        }

        public bool HasCoins(BigInteger coins) => GetCoins() >= coins;
        
        public BigInteger GetCoins() => _coins;

        public void AddCoins(BigInteger coinsDelta) =>
            SetCoins(_coins + coinsDelta);

        public void SubtractCoins(BigInteger coinsDelta) =>
            SetCoins(_coins - coinsDelta);

        public void SetCoins(BigInteger coins)
        {
            _coins = coins;
            _gameSaves.SaveCoins(coins);
            OnCoinsChange?.Invoke(coins);
        }
    }
}