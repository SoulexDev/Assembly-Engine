using AssemblyEngine.SceneManagement;

namespace AssemblyEngine
{
    public class EngineObjectFactory
    {
        public static EngineObject Instantiate(string name)
        {
            EngineObject obj = new EngineObject(name);

            SceneManager.activeScene.engineObjects.Add(obj);
            return obj;
        }
    }
}
