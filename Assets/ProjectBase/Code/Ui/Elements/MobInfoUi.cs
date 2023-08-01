using System.Numerics;
using Game.Mobs;
using TMPro;
using UnityEngine;
using Utility;

namespace Ui.Elements
{
    public class MobInfoUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private ProgressBar healthBar;
        
        private MobSpawner _mobSpawner;

        [Zenject.Inject]
        public void Construct(MobSpawner mobSpawner, MobDeathController mobDeathController)
        {
            _mobSpawner = mobSpawner;
            _mobSpawner.OnMobSpawn += OnMobSpawn;
            _mobSpawner.OnMobDespawn += OnMobDespawn;
            mobDeathController.OnMobDeath += OnMobDeath;
        }

        public void OnGameStart()
        {
        }

        private void OnMobSpawn(Mob mob)
        {
            mob.OnHealthChange += OnMobHealthChange;

            nameText.text = $"Mob {mob.Id}";
            
            healthBar.SetActive(true);
            UpdateMobHealthBar(mob);
        }

        private void OnMobDeath(Mob mob)
        {
            nameText.text = "Mob dead";
        }

        private void OnMobDespawn(Mob mob)
        {
            mob.OnHealthChange -= OnMobHealthChange;
            
            healthBar.SetActive(false);
        }

        private void OnMobHealthChange(Mob mob, BigInteger prevHealth)
        {
            UpdateMobHealthBar(mob);
        }
        
        private void UpdateMobHealthBar(Mob mob)
        {
            healthBar.SetValue(BigIntegerX.RationalDivision(mob.Health, mob.MaxHealth));
        }
    }
}