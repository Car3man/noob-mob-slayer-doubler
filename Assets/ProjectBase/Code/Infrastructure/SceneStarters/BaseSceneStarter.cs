using UnityEngine;

namespace Infrastructure.SceneStarters
{
    public abstract class BaseSceneStarter : MonoBehaviour
    {
        private ProjectEntryPoint _projectEntryPoint;

        [Zenject.Inject]
        public void Construct(ProjectEntryPoint projectEntryPoint)
        {
            _projectEntryPoint = projectEntryPoint;
        }
        
        private void Awake()
        {
            if (!_projectEntryPoint.EntryFinished)
            {
                return;
            }
            
            OnStart();
        }

        protected abstract void OnStart();
    }
}