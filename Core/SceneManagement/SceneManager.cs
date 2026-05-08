using System.Text.Json;

namespace AssemblyEngine.SceneManagement
{
    public class SceneManager
    {
        internal static Scene activeScene;
        internal static List<Scene> loadedScenes = new List<Scene>();
        //TODO: add functionality for additional scenes
        public static void SetActiveScene(string name)
        {

        }
        public static void SetActiveScene(Scene scene)
        {
            if (!loadedScenes.Contains(scene))
            {
                loadedScenes.Add(scene);
                activeScene = scene;
            }
        }
        public static void LoadScene(string sceneName, bool additional = false)
        {
            //JsonDocument sceneDoc = 
            //JsonSerializer.Deserialize<Scene>();
        }
        public static void SaveScene(Scene scene)
        {
            if (scene == null)
                return;

            //JsonSerializer.Serialize(scene, JsonSerializerOptions.Default);
        }
        public static Scene GetActiveScene()
        {
            return activeScene;
        }
    }
}