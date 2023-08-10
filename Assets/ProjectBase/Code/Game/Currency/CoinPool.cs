using Zenject;

namespace Game.Currency
{
    public class CoinPool : MonoMemoryPool<Coin>
    {
        protected override void Reinitialize(Coin coinItem)
        {
            coinItem.ResetForPool();
        }
    }
}