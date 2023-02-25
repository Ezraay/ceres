using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] [Scene] private string queueScene;

        public void JoinQueue()
        {
            SceneManager.LoadScene(queueScene);
        }

        public void Quit()
        {
            Application.Quit(); 
        }
    }
}