using System.Numerics;
using Newtonsoft.Json;

namespace Game.Configs.JsonImpl
{
    public class IslandJsonObject
    {
        public string Id;
        public int Number;
        public string ParentId;
        public string ResId;
        public string[] MobsIds;
        public int? CountMobsToComplete;
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger? MobsHealth;
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger? MobsCoinsReward;
        public bool? IsTimeLimit;
        public float? TimeLimit;
    }
}