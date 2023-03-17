using System.Threading.Tasks;
using UnityEngine;

namespace Ceres.Client
{
    public static class SceneManager
    {
        public static string CurrentScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        public static async Task LoadScene(string scene)
        {
            AsyncOperation task = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
            while (!task.isDone)
            {
                await Task.Yield();
            }
        }
        
    //     bool succeeded = false;
    //         while (!succeeded)
    //     {
    //         // do work
    //         succeeded = outcome; // if it worked, make as succeeded, else retry
    //         await Task.Delay(1000); // arbitrary delay
    //     }
    // return succeeded;
    }

    public class GameScene
    {
        public const string
            Initialise = "Initialise",
            MainMenu = "Main Menu",
            Queue = "Queue",
            Battle = "Battle";
    }
}