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
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private ProgressBar healthBar;
        
        private MobSpawner _mobSpawner;

        [Zenject.Inject]
        public void Construct(MobSpawner mobSpawner, MobDeathHandler mobDeathHandler)
        {
            _mobSpawner = mobSpawner;
            _mobSpawner.OnMobSpawn += OnMobSpawn;
            _mobSpawner.OnMobDespawn += OnMobDespawn;
            mobDeathHandler.OnMobDeath += OnMobDeath;
        }

        public void OnGameStart()
        {
        }

        private void OnMobSpawn(Mob mob)
        {
            mob.OnHealthChange += OnMobHealthChange;

            nameText.text = $"{mob.Prototype.Id}";
            healthText.text = FormatMobHealth(mob);
            healthBar.SetActive(true);
            UpdateMobHealthBar(mob);
        }

        private void OnMobDeath(Mob mob)
        {
            nameText.text = "Dead";
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
            healthText.text = FormatMobHealth(mob);
            healthBar.SetValue(BigIntegerX.RationalDivision(mob.Health, mob.MaxHealth));
        }

        private string FormatMobHealth(Mob mob)
        {
            return $"{mob.Health} HP";
        }
    }
}