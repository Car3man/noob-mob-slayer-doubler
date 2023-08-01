using System.Numerics;
using Newtonsoft.Json;

namespace Game.Configs.JsonImpl
{
    public class MobJsonObject
    {
        public int Id;
        public int? ParentId;
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger? BaseHealth;
    }
}