using UnityEngine;
using NaughtyAttributes;

namespace Ceres.Client
{
    public class GameInitialiser : MonoBehaviour
    {
        [SerializeField] private NetworkManager networkManager;
        [SerializeField, Scene] private string nextScene;
        
        
        private void Start()
        {
            networkManager.OnConnected += LoadNextScene;
            networkManager.Connect();
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}