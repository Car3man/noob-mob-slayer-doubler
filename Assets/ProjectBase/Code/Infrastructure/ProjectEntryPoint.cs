using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure
{
    public class ProjectEntryPoint : IInitializable
    {
        public bool EntryFinished { get; private set; }
        
        public void Initialize()
        {
            EntryFinished = true;
            SceneManager.LoadScene("ProjectBase/Scenes/Splash");
        }
    }
}