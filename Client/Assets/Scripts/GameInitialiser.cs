using Ceres.Client.BattleSystem;
using UnityEngine;
using NaughtyAttributes;
using Zenject;

namespace Ceres.Client
{
    public class GameInitialiser : MonoBehaviour
    {
        // [SerializeField] private NetworkManager networkManager;
        [SerializeField, Scene] private string nextScene;


        [Inject]
        public void Construct(NetworkManager networkManager)
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