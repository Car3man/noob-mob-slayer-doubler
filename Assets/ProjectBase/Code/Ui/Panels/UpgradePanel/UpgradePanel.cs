using System.Collections.Generic;
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
        
        private UpgradeController _upgradeController;
        private readonly Dictionary<int, UpgradeItem> _upgradeItems = new();

        public bool IsOpened { get; private set; }

        [Zenject.Inject]
        public void Construct(UpgradeController upgradeController)
        {
            _upgradeController = upgradeController;
        }

        public void OnGameStart()
        {
            upgradeItemTemplate.gameObject.SetActive(false);
            
            _upgradeController.OnBuyUpgrade += OnBuyUpgrade;
            
            toggleOpenedButton.onClick.AddListener(ToggleShowState);
            toggleClosedButton.onClick.AddListener(ToggleShowState);

            CreateUpgradeItems();
            ApplyOpenedState();
        }

        private void OnDestroy()
        {
            _upgradeController.OnBuyUpgrade -= OnBuyUpgrade;
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
            foreach (var upgrade in _upgradeController.GetUpgrades())
            {
                var upgradeItem = CreateUpgradeItem();
                upgradeItem.OnUpgradeButtonClick += OnUpgradeButtonClick;
                upgradeItem.SetUpgrade(upgrade);
                _upgradeItems.Add(upgrade.Id, upgradeItem);
            }
        }

        private UpgradeItem CreateUpgradeItem()
        {
            var upgradeItem = Instantiate(upgradeItemTemplate, upgradeItemsParent);
            upgradeItem.gameObject.SetActive(true);
            return upgradeItem;
        }

        private void OnUpgradeButtonClick(Upgrade upgrade)
        {
            _upgradeController.BuyUpgrade(upgrade.Id);
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
