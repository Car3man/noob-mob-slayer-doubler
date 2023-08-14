using Game.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.Panels
{
    public class UpgradeItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private UpgradeButton upgradeButton;
        
        private UpgradeInventory _upgradeInventory;

        public Upgrade Upgrade { get; private set; }
        
        public delegate void UpgradeButtonClickDelegate(Upgrade upgrade);
        public event UpgradeButtonClickDelegate OnUpgradeButtonClick;

        [Zenject.Inject]
        public void Construct(UpgradeInventory upgradeInventory)
        {
            _upgradeInventory = upgradeInventory;
        }
        
        private void OnEnable()
        {
            upgradeButton.OnClick += OnButtonUpgradeClick;
        }

        private void OnDisable()
        {
            upgradeButton.OnClick -= OnButtonUpgradeClick;
        }

        public void SetUpgrade(Upgrade upgrade)
        {
            Upgrade = upgrade;
            UpdateUpgradeInfo();
        }

        public void NotifyAboutUpgradeLevelChange()
        {
            UpdateUpgradeInfo();
        }

        private void UpdateUpgradeInfo()
        {
            titleText.text = $"Upgrade - {Upgrade.Prototype.Id}";
            levelText.text = $"Lvl.{Upgrade.Level}";
            damageText.text = _upgradeInventory.GetDamageByUpgradeId(Upgrade.Prototype.Id).ToString();
            upgradeButton.SetUpgradeInfo(Upgrade, 1);
        }
        
        private void OnButtonUpgradeClick()
        {
            OnUpgradeButtonClick?.Invoke(Upgrade);
        }
        
        public class Factory : PlaceholderFactory<UpgradeItem>
        {
            
        }
    }
}