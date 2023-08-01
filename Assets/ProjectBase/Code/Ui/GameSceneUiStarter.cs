using Ui.Elements;
using Ui.Panels;
using UnityEngine;

namespace Ui
{
    public class GameSceneUiStarter : MonoBehaviour
    {
        [SerializeField] private CurrencyInfoUi currencyInfo;
        [SerializeField] private IslandInfoUi islandInfoUi;
        [SerializeField] private MobInfoUi mobInfo;
        [SerializeField] private UpgradePanel upgradePanel;

        public void OnGameStart()
        {
            currencyInfo.OnGameStart();
            islandInfoUi.OnGameStart();
            mobInfo.OnGameStart();
            upgradePanel.OnGameStart();
        }
    }
}