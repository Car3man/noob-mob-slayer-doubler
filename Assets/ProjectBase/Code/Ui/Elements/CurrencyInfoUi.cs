using Game.Currency;
using TMPro;
using UnityEngine;

namespace Ui.Elements
{
    public class CurrencyInfoUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;
        
        private CurrencyController _currencyController;

        [Zenject.Inject]
        public void Construct(CurrencyController currencyController)
        {
            _currencyController = currencyController;
            _currencyController.OnCoinsChange += _ => { UpdateCoinsText(); };
        }

        public void OnGameStart()
        {
            UpdateCoinsText();
        }

        private void UpdateCoinsText()
        {
            coinsText.text = _currencyController.GetCoins().ToString();
        }
    }
}
