using System.Numerics;
using Newtonsoft.Json;

namespace Game.Configs.JsonImpl
{
    public class UpgradeJsonObject
    {
        public int Id;
        public string Type;
        public int StartLevel;
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger BaseDamage;
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger BasePrice;
        public float PriceMultiplier;
    }
}