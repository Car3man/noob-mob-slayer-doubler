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
        [SerializeField] private Transform upgradeItemsParent;

        private UpgradeItem.Factory _itemFactory;
        private UpgradeInventory _upgradeInventory;
        private readonly Dictionary<int, UpgradeItem> _upgradeItems = new();

        public bool IsOpened { get; private set; } = true;

        [Zenject.Inject]
        public void Construct(
            UpgradeItem.Factory itemFactory,
            UpgradeInventory upgradeInventory
            )
        {
            _itemFactory = itemFactory;
            _upgradeInventory = upgradeInventory;
        }

        public void OnGameStart()
        {
            _upgradeInventory.OnUpgradeLevelChange += OnUpgradeLevelChange;
            
            toggleOpenedButton.onClick.AddListener(ToggleShowState);
            toggleClosedButton.onClick.AddListener(ToggleShowState);

            CreateUpgradeItems();
            ApplyOpenedState();
        }

        private void OnDestroy()
        {
            _upgradeInventory.OnUpgradeLevelChange -= OnUpgradeLevelChange;
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
            var upgradeItem = _itemFactory.Create();
            upgradeItem.transform.SetParent(upgradeItemsParent);
            return upgradeItem;
        }

        private void UpdateUpgradeItemsActiveState()
        {
            foreach (var upgradeItem in _upgradeItems.Values)
            {
                var isAvailableToBuy = _upgradeInventory.IsUpgradeUnlocked(upgradeItem.Upgrade.Prototype.Id);
                upgradeItem.gameObject.SetActive(isAvailableToBuy);
            }
        }

        private void OnUpgradeButtonClick(Upgrade upgrade)
        {
            _upgradeInventory.BuyUpgrade(upgrade.Prototype.Id);
        }

        private void OnUpgradeLevelChange(int upgradeId)
        {
            UpdateUpgradeItemsActiveState();
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
