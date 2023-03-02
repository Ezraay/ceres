namespace Ceres.Client
{
    public static class SceneManager
    {
        public static string CurrentScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        public static void LoadScene(string scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
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