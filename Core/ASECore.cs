using AssemblyEngine.Graphics;
using AssemblyEngine.GUI;
using AssemblyEngine.Physics;
using AssemblyEngine.SceneManagement;
using AssemblyEngine.Testing;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using static SDL3.SDL;

namespace AssemblyEngine
{
    //TODO: for soup jam, we need scenes, proper internal/external render pipeline components, audio, more robust physics, sprites, particles, billboards
    public class ASECore
    {
        public static string AssetsPath;
        public static string RawPath;

        public delegate void AppInit();
        public static event AppInit OnAppInit;

        public static Shader defaultShader;
        public static Shader defaultParticleShader;
        public static Shader defaultUIShader;

        //internal static List<EngineObject> engineObjects = new List<EngineObject>();

        private static Stopwatch fixedStepTimer;
        private static long timerOffset;

        public static void Main(string[] args)
        {
            ASECore core = new ASECore();
            core.Init();
        }
        public int Init()
        {
            AssetsPath = AppDomain.CurrentDomain.BaseDirectory;
            RawPath = AssetsPath;
            AssetsPath = Path.Combine(AssetsPath, "resources");
            Console.WriteLine(AssetsPath);
            return SDL_RunApp(0, 0, SDL_Main, 0);
        }
        unsafe int SDL_Main(int argc, nint argv)
        {
            return SDL_EnterAppMainCallbacks(argc, argv, SDL_AppInit, SDL_AppIterate, SDL_AppEvent, SDL_AppQuit);
        }
        static SDL_AppResult SDL_AppInit(nint appState, int argc, nint argv)
        {
            //initialize core functionalities
            if (!RenderPipeline.Init())
            {
                return SDL_AppResult.SDL_APP_FAILURE;
            }
            GUIManager.Init();
            //GUIManager.EnableGUI();

            Scene scene = new Scene("Untitled Scene");
            SceneManager.SetActiveScene(scene);

            fixedStepTimer = new Stopwatch();
            fixedStepTimer.Start();

            PhysicsSimulation.Init();
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

            //TODO: special internal stuff for rendering to allow users to interface with the rendering system in a managable way
            //users shouldn't have to set the light resolution everytime. it should be global. they should still be able to change the resolution per light, though
            //there should be camera components and interal camera. camera components interface with internal cameras

            EngineObject freeCam = EngineObjectFactory.Instantiate("Free Cam");
            freeCam.AddComponent<FreeCam>("free cam 1");

            //ParticleManager.EmitParticles(particlesTex);

            OnAppInit?.Invoke();
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        static SDL_AppResult SDL_AppIterate(nint appstate)
        {
            Input.SetCurrentState();

            Time.Update();
            Input.Update();

            foreach (var scene in SceneManager.loadedScenes)
            {
                foreach (var obj in scene.engineObjects)
                {
                    obj.components.ForEach(c => c.Update());
                }
                foreach (var obj in scene.engineObjects)
                {
                    obj.components.ForEach(c => c.LateUpdate());
                }
            }

            if (fixedStepTimer.ElapsedMilliseconds + timerOffset > PhysicsSimulation.millisecondsPerTick)
            {
                timerOffset = fixedStepTimer.ElapsedMilliseconds + timerOffset - PhysicsSimulation.millisecondsPerTick;
                fixedStepTimer.Restart();

                Time.FixedUpdate();

                foreach (var scene in SceneManager.loadedScenes)
                {
                    foreach (var obj in scene.engineObjects)
                    {
                        obj.components.ForEach(c => c.PhysicsTick());
                    }
                }
                
                PhysicsSimulation.Tick();
            }
            //ParticleManager.UpdateParticles();

            RenderPipeline.Render();

            //Input.SetLastState();
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        unsafe static SDL_AppResult SDL_AppEvent(nint appstate, SDL_Event* sdlEvent)
        {
            Input.OnEvent();
            RenderPipeline.OnEvent();
            GUIManager.HandleEvents(sdlEvent);

            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        static void SDL_AppQuit(nint appstate, SDL_AppResult result)
        {
            RenderPipeline.Dispose();
            PhysicsSimulation.Dispose();

            SDL_Quit();
        }
    }
}