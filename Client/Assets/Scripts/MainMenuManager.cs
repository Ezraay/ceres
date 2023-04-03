using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class MainMenuManager : MonoBehaviour
    {
        private SceneManager sceneManager;
        
        [Inject]
        public void Construct(SceneManager scene)
        {
            this.sceneManager = scene;
        }
        
        public void JoinQueue()
        {
            sceneManager.LoadScene(new QueueScene());
        }

        public void Quit()
        {
            Application.Quit(); 
        }
    }
}