using Game.Mobs;
using UnityEngine;

namespace Game.Islands
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private GameObject mobSpawnPoint;
        
        private IslandPrototype _prototype;

        public int Id => _prototype.Id;
        public MobPrototype[] Mobs => _prototype.Mobs;
        public GameObject MobSpawnPoint => mobSpawnPoint;

        [Zenject.Inject]
        public void Construct(IslandPrototype prototype)
        {
            _prototype = prototype;
        }
    }
}