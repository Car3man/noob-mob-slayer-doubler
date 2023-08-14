using System.Linq;
using Game.Configs;
using Game.Islands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Elements
{
    public class IslandInfoUi : MonoBehaviour
    {
        [SerializeField] private Button changeIslandLeft;
        [SerializeField] private Button changeIslandRight;
        [SerializeField] private TextMeshProUGUI islandNameText;
        [SerializeField] private GameObject completeProgress;
        [SerializeField] private TextMeshProUGUI completeProgressText;
        [SerializeField] private GameObject timeLimit;
        [SerializeField] private TextMeshProUGUI timeLimitText;

        private IConfigProvider _configProvider;
        private IslandChanger _islandChanger;
        private IslandCompletionSystem _islandCompletionSystem;
        private IslandLimitTimer _islandLimitTimer;
        
        private bool _gameStarted;

        [Zenject.Inject]
        public void Construct(
            IConfigProvider configProvider,
            IslandChanger islandChanger,
            IslandCompletionSystem islandCompletionSystem,
            IslandLimitTimer islandLimitTimer
            )
        {
            _configProvider = configProvider;
            _islandChanger = islandChanger;
            _islandCompletionSystem = islandCompletionSystem;
            _islandLimitTimer = islandLimitTimer;
            
            _islandChanger.OnIslandChange += OnIslandChange;
            _islandCompletionSystem.OnIslandCompleteProgress += OnIslandCompletionProgress;
        }

        private void OnEnable()
        {
            changeIslandLeft.onClick.AddListener(OnButtonChangeIslandLeftClick);
            changeIslandRight.onClick.AddListener(OnButtonChangeIslandRightClick);
        }

        private void OnDisable()
        {
            changeIslandLeft.onClick.RemoveListener(OnButtonChangeIslandLeftClick);
            changeIslandRight.onClick.RemoveListener(OnButtonChangeIslandRightClick);
        }

        private void Update()
        {
            timeLimit.SetActive(_islandLimitTimer.IsActive);
            if (_islandLimitTimer.IsActive)
            {
                timeLimitText.text = $"{_islandLimitTimer.TimeDown:F1}";
            }
        }

        public void OnGameStart()
        {
        }

        private void OnIslandChange()
        {
            UpdateIslandName();
            UpdateCompleteProgress();
        }

        private void OnIslandCompletionProgress(int islandId, int currValue, int valueToReach)
        {
            UpdateCompleteProgress();
        }

        private void UpdateIslandName()
        {
            var currentIsland = _islandChanger.CurrentIsland;
            if (currentIsland != null)
            {
                var maxIslandNumber = _configProvider
                    .GetIslands()
                    .OrderBy(x => x.Number)
                    .Last()
                    .Number;
                var islandName = "Island";
                var currentIslandPrototype = currentIsland.Prototype;
                islandNameText.text = $"{islandName} " +
                                      $"Lvl.{(currentIslandPrototype.Number == maxIslandNumber ? "Max" : currentIslandPrototype.Number)}";
            }
            else
            {
                islandNameText.text = string.Empty;
            }
        }

        private void UpdateCompleteProgress()
        {
            var currentIsland = _islandChanger.CurrentIsland;
            if (currentIsland == null)
            {
                completeProgress.SetActive(false);
                return;
            }

            var currentIslandPrototype = currentIsland.Prototype;
            var (currProgress, progressToReach) = _islandCompletionSystem.GetProgressByIslandNumber(currentIslandPrototype.Number);
            if (currProgress >= progressToReach)
            {
                completeProgress.SetActive(false);
                return;
            }
            
            completeProgress.SetActive(true);
            completeProgressText.text = $"{currProgress}/{progressToReach}";
        }
        
        private void OnButtonChangeIslandLeftClick()
        {
            var currIslandNumber = _islandChanger.CurrentIsland.Prototype.Number;
            _islandChanger.ChangeAndSaveIsland(currIslandNumber - 1);
        }

        private void OnButtonChangeIslandRightClick()
        {
            var currIslandNumber = _islandChanger.CurrentIsland.Prototype.Number;
            _islandChanger.ChangeAndSaveIsland(currIslandNumber + 1);
        }
    }
}