using AssemblyEngine.Graphics;
using System.Text.Json.Serialization;

namespace AssemblyEngine.SceneManagement
{
    [JsonSerializable(typeof(Scene), GenerationMode = JsonSourceGenerationMode.Default)]
    public class Scene
    {
        public string name = "Untitled Scene";

        public LightingSettings lightingSettings;
        public List<EngineObject> engineObjects = new List<EngineObject>();

        public Scene(string name)
        {
            this.name = name;
            lightingSettings = new LightingSettings();
        }
    }
}