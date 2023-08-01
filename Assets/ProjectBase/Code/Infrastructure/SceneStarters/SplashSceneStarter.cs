using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Saves;
using UnityEngine.SceneManagement;

namespace Infrastructure.SceneStarters
{
    public class SplashSceneStarter : BaseSceneStarter
    {
        private IConfigProvider _configProvider;
        private GameSaves _gameSaves;

        [Zenject.Inject]
        public void Construct(IConfigProvider configProvider, GameSaves gameSaves)
        {
            _configProvider = configProvider;
            _gameSaves = gameSaves;
        }
        
        protected override async void OnStart()
        {
            await LoadConfigs();
            await LoadSaves();
            await LoadGameScene();
        }

        private async Task LoadConfigs()
        {
            await _configProvider.LoadAllAsync();
        }

        private async Task LoadSaves()
        {
            await _gameSaves.LoadAllAsync();
        }

        private async Task LoadGameScene()
        {
            await SceneManager.LoadSceneAsync("ProjectBase/Scenes/Game");
        }
    }
}