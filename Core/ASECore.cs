using AssemblyEngine.Graphics;
using AssemblyEngine.Testing;
using OpenTK.Graphics.OpenGL4;
using static SDL3.SDL;

namespace AssemblyEngine
{
    public class ASECore
    {
        public static string AssetsPath;

        public delegate void AppInit();
        public static event AppInit OnAppInit;

        public static Shader defaultShader;
        public static Shader defaultParticleShader;
        public static Shader defaultUIShader;

        internal static List<EngineObject> engineObjects = new List<EngineObject>();

        //public static void Main(string[] args)
        //{
        //    ASECore core = new ASECore();
        //    core.Init();
        //}
        public int Init()
        {
            AssetsPath = AppDomain.CurrentDomain.BaseDirectory;
            AssetsPath = Path.Combine(AssetsPath, "resources");
            Console.WriteLine(AssetsPath);
            return SDL_RunApp(0, 0, SDL_Main, 0);
        }
        unsafe int SDL_Main(int argc, nint argv)
        {
            return SDL_EnterAppMainCallbacks(argc, argv, SDL_AppInit, SDL_AppIterate, SDL_AppEvent, SDL_AppQuit);
        }
        static SDL_AppResult SDL_AppInit(nint appState, int argc, nint argv){
            //initialize core functionalities
            if (!RenderPipeline.Init())
            {
                return SDL_AppResult.SDL_APP_FAILURE;
            }
            Input.Init();

            //create internal objects
            ResourceLoader.LoadResource(out defaultShader,
                ("internal/shaders/simple_lit", ShaderType.VertexShader),
                ("internal/shaders/simple_lit", ShaderType.FragmentShader));

            ResourceLoader.LoadResource(out defaultParticleShader, 
                ("internal/shaders/particle_unlit", ShaderType.VertexShader),
                ("internal/shaders/particle_unlit", ShaderType.FragmentShader));

            ResourceLoader.LoadResource(out defaultUIShader,
                ("internal/shaders/ui", ShaderType.VertexShader),
                ("internal/shaders/ui", ShaderType.FragmentShader));

            EngineObject obj = EngineObjectFactory.Instantiate();
            obj.AddComponent<FreeCam>("free cam 1");

            //ParticleManager.EmitParticles(particlesTex);

            OnAppInit?.Invoke();
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        static SDL_AppResult SDL_AppIterate(nint appstate){
            Input.SetCurrentState();

            Time.Update();
            Input.Update();

            foreach (var obj in engineObjects)
            {
                obj.components.ForEach(c => c.Update());
            }
            foreach (var obj in engineObjects)
            {
                obj.components.ForEach(c => c.LateUpdate());
            }

            //ParticleManager.UpdateParticles();

            RenderPipeline.Render();

            //Input.SetLastState();
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        unsafe static SDL_AppResult SDL_AppEvent(nint appstate, SDL_Event* sdlEvent){
            Input.OnEvent();
            RenderPipeline.OnEvent();

            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        static void SDL_AppQuit(nint appstate, SDL_AppResult result){
            RenderPipeline.Dispose();

            SDL_Quit();
        }
    }
}