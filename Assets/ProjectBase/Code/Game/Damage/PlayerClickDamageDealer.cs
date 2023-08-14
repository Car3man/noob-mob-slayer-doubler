using Game.PlayerInput;
using Game.Upgrades;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Game.Damage
{
    public class PlayerClickDamageDealer : MonoBehaviour
    {
        [SerializeField] private ObjectInputHandler objectInputHandler;
        
        private DamageDealer _damageDealer;
        private UpgradeInventory _upgradeInventory;

        [Zenject.Inject]
        public void Construct(
            DamageDealer damageDealer,
            UpgradeInventory upgradeInventory
            )
        {
            _damageDealer = damageDealer;
            _upgradeInventory = upgradeInventory;
        }

        private void Start()
        {
            Assert.IsNotNull(objectInputHandler);
        }

        private void OnEnable()
        {
            objectInputHandler.OnClick += HandleDamageClick;
        }

        private void OnDisable()
        {
            objectInputHandler.OnClick -= HandleDamageClick;
        }

        private void HandleDamageClick(PointerEventData eventData)
        {
            var clickDamage = _upgradeInventory.GetDamageByUpgradeType(UpgradeType.Click);
            _damageDealer.DealDamage(clickDamage, true);
        }
    }
}