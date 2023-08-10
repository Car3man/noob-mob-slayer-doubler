using System.Collections.Generic;
using System.Linq;
using Game.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Panels
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private RectTransform panelRectTransform;
        [SerializeField] private Button toggleOpenedButton;
        [SerializeField] private Button toggleClosedButton;
        [SerializeField] private UpgradeItem upgradeItemTemplate;
        [SerializeField] private Transform upgradeItemsParent;
        
        private UpgradeInventory _upgradeInventory;
        private readonly Dictionary<int, UpgradeItem> _upgradeItems = new();

        public bool IsOpened { get; private set; }

        [Zenject.Inject]
        public void Construct(UpgradeInventory upgradeInventory)
        {
            _upgradeInventory = upgradeInventory;
        }

        public void OnGameStart()
        {
            upgradeItemTemplate.gameObject.SetActive(false);
            
            _upgradeInventory.OnBuyUpgrade += OnBuyUpgrade;
            
            toggleOpenedButton.onClick.AddListener(ToggleShowState);
            toggleClosedButton.onClick.AddListener(ToggleShowState);

            CreateUpgradeItems();
            ApplyOpenedState();
        }

        private void OnDestroy()
        {
            _upgradeInventory.OnBuyUpgrade -= OnBuyUpgrade;
            toggleOpenedButton.onClick.RemoveListener(ToggleShowState);
            toggleClosedButton.onClick.RemoveListener(ToggleShowState);
        }

        public void ToggleShowState()
        {
            IsOpened = !IsOpened;
            ApplyOpenedState();
        }

        private void CreateUpgradeItems()
        {
            foreach (var upgrade in _upgradeInventory.GetUpgrades().OrderBy(upgrade => upgrade.Prototype.Id))
            {
                var upgradeItem = CreateUpgradeItem();
                upgradeItem.OnUpgradeButtonClick += OnUpgradeButtonClick;
                upgradeItem.SetUpgrade(upgrade);
                _upgradeItems.Add(upgrade.Prototype.Id, upgradeItem);
            }
            
            UpdateUpgradeItemsActiveState();
        }

        private UpgradeItem CreateUpgradeItem()
        {
            var upgradeItem = Instantiate(upgradeItemTemplate, upgradeItemsParent);
            return upgradeItem;
        }

        private void UpdateUpgradeItemsActiveState()
        {
            foreach (var upgradeItem in _upgradeItems.Values)
            {
                var isAvailableToBuy = _upgradeInventory.IsUpgradeAvailableToBuy(upgradeItem.Upgrade.Prototype.Id);
                upgradeItem.gameObject.SetActive(isAvailableToBuy);
            }
        }

        private void OnUpgradeButtonClick(Upgrade upgrade)
        {
            _upgradeInventory.BuyUpgrade(upgrade.Prototype.Id);
            UpdateUpgradeItemsActiveState();
        }

        private void OnBuyUpgrade(int upgradeId)
        {
            _upgradeItems[upgradeId].NotifyAboutUpgradeLevelChange();
        }

        private void ApplyOpenedState()
        {
            UpdateToggleButtons();
            UpdatePanelPosition();
        }

        private void UpdateToggleButtons()
        {
            toggleOpenedButton.gameObject.SetActive(IsOpened);
            toggleClosedButton.gameObject.SetActive(!IsOpened);
        }

        private void UpdatePanelPosition()
        {
            var panelWidth = panelRectTransform.rect.width;
            var panelPos = panelRectTransform.anchoredPosition;
            panelPos.x = IsOpened ? 0 : -panelWidth;
            panelRectTransform.anchoredPosition = panelPos;
        }
    }
}
