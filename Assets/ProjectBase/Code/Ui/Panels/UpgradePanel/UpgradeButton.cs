using System;
using Game.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Panels
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI upgradeText;
        [SerializeField] private TextMeshProUGUI priceText;

        public delegate void ClickDelegate();
        public event ClickDelegate OnClick;

        private void OnEnable()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        public void SetUpgradeInfo(Upgrade upgrade, int upgradeCount)
        {
            // TODO: implement upgrade counts, etc
            
            upgradeText.text = "Level +";
            priceText.text = upgrade.GetUpgradePrice().ToString();
        }

        private void OnButtonClick()
        {
            OnClick?.Invoke();
        }
    }
}