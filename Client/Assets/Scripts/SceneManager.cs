namespace Ceres.Client
{
    public static class SceneManager
    {
        public static void LoadScene(string scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }
}