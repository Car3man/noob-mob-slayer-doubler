using Game.Input;
using Game.Upgrades;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Game.Damage
{
    public class PlayerClickDamageDealer : MonoBehaviour
    {
        [SerializeField] private ObjectInputHandler objectInputHandler;
        
        private MobDamageDealer _mobDamageDealer;
        private UpgradeController _upgradeController;

        [Zenject.Inject]
        public void Construct(
            MobDamageDealer mobDamageDealer,
            UpgradeController upgradeController
            )
        {
            _mobDamageDealer = mobDamageDealer;
            _upgradeController = upgradeController;
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
            var clickDamage = _upgradeController.GetDamageByUpgradeType(UpgradeType.Click);
            _mobDamageDealer.DealDamage(clickDamage);
        }
    }
}