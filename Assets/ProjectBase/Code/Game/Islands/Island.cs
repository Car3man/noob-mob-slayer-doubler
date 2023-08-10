using UnityEngine;

namespace Game.Islands
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private GameObject mobSpawnPoint;
        
        private IslandPrototype _prototype;

        public IslandPrototype Prototype => _prototype;
        public GameObject MobSpawnPoint => mobSpawnPoint;

        [Zenject.Inject]
        public void Construct(IslandPrototype prototype)
        {
            _prototype = prototype;
        }
    }
}