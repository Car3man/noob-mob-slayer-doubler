using System.Numerics;
using UnityEngine;

namespace Game.Currency
{
    public class Coin : MonoBehaviour
    {
        public bool WasTook { get; private set; }
        public BigInteger Value { get; private set; }

        public void ResetForPool()
        {
            WasTook = false;
            Value = 0;
        }

        public void MarkWasTook()
        {
            WasTook = true;
        }

        public void SetValue(BigInteger value)
        {
            Value = value;
        }
    }
}
