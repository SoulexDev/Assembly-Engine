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

        public static void Main(string[] args)
        {
            ASECore core = new ASECore();
            core.Init();
        }
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

            //ECSManager.Init();
            GL.LineWidth(1.5f);

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

            //ResourceLoader.LoadResource(out Texture2D checkerDark, "internal/textures/Prototype_Dark.png");
            //ResourceLoader.LoadResource(out Texture2D checkerLight, "internal/textures/Prototype_Light.png");
            //ResourceLoader.LoadResource(out Texture2D particlesTex, "internal/textures/gaussian_circle_1.png");
            //ResourceLoader.LoadResource(out Texture2D guitarTex, "internal/models/1001_albedo.jpg");

            //Plane plane = PlaneGenerator.Generate(checkerLight, PrimitiveType.Triangles, 25, 25, 100);
            //plane.transform.scale = Vector3.One * 4;

            //ResourceLoader.LoadResource(out Renderable guitar, "internal/models/guitartypeshi.fbx");
            //guitar.transform.position = Vector3.UnitY * 4;

            //Material guitarMat = new Material(defaultShader);
            //guitarMat.texture2Ds.Add(("uMainTex", guitarTex));

            //guitar.SetMaterial(guitarMat);

            //for (int i = 0; i < 8; i++)
            //{
            //    Cube cube = new Cube(checkerDark);
            //    cube.transform.position = 16 * new Vector3((float)MathHelper.Cos(i / 8f * MathHelper.TwoPi), 0, (float)MathHelper.Sin(i / 8f * MathHelper.TwoPi));
            //    cube.transform.scale = new Vector3(2, 40, 2);
            //}

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
                foreach (var component in obj.components)
                {
                    component.Update();
                }
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