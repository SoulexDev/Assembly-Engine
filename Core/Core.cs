using AssemblyEngine.ECS;
using AssemblyEngine.Graphics;
using AssemblyEngine.Graphics.Testing;
using AssemblyEngine.Testing;
using AssemblyEngine.UI;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static SDL3.SDL;

namespace AssemblyEngine
{
    public class Core
    {
        public static string AssetsPath;
        private static FreeCam freecam;

        public static Shader defaultShader;
        public static Shader defaultParticleShader;
        public static Shader defaultUIShader;

        public static ASXMLCanvas canvas;
        //private static Transform shrekT;

        public static void Main(string[] args)
        {
            AssetsPath = AppDomain.CurrentDomain.BaseDirectory;
            AssetsPath = Path.Combine(AssetsPath, "resources");
            Console.WriteLine(AssetsPath);
            SDL_EnterAppMainCallbacks(0, 0, SDL_AppInit, SDL_AppIterate, SDL_AppEvent, SDL_AppQuit);
        }
        static SDL_AppInit_func SDL_AppInit = (nint appState, int argc, nint argv) => {
            //initialize core functionalities
            if (!RenderPipeline.Init())
            {
                return SDL_AppResult.SDL_APP_FAILURE;
            }
            Input.Init();

            //ECSManager.Init();

            QuadMesh.Create();
            GL.LineWidth(1.5f);

            //create internal objects
            ResourceLoader.LoadResource(out defaultShader,
                ("shaders/internal/simple_lit", ShaderType.VertexShader),
                ("shaders/internal/simple_lit", ShaderType.FragmentShader));

            ResourceLoader.LoadResource(out defaultParticleShader, 
                ("shaders/internal/particle_unlit", ShaderType.VertexShader),
                ("shaders/internal/particle_unlit", ShaderType.FragmentShader));

            ResourceLoader.LoadResource(out defaultUIShader,
                ("shaders/internal/ui", ShaderType.VertexShader),
                ("shaders/internal/ui", ShaderType.FragmentShader));

            freecam = new FreeCam(Camera.main);

            ResourceLoader.LoadResource(out Texture2D checkerDark, "textures/Prototype_Dark.png");
            ResourceLoader.LoadResource(out Texture2D checkerLight, "textures/Prototype_Light.png");
            ResourceLoader.LoadResource(out Texture2D particlesTex, "textures/gaussian_circle_1.png");
            ResourceLoader.LoadResource(out Texture2D guitarTex, "models/1001_albedo.jpg");

            Plane plane = PlaneGenerator.Generate(checkerLight, PrimitiveType.Triangles, 25, 25, 100);
            plane.transform.scale = Vector3.One * 4;

            ResourceLoader.LoadResource(out Renderable guitar, "models/guitartypeshi.fbx");
            guitar.transform.position = Vector3.UnitY * 4;

            Material guitarMat = new Material(defaultShader);
            guitarMat.texture2Ds.Add(("uMainTex", guitarTex));

            guitar.SetMaterial(guitarMat);

            Cube cube = new Cube(checkerDark);
            cube.transform.position = Vector3.UnitX * 4;
            cube.transform.scale = new Vector3(2, 40, 2);

            //ParticleManager.EmitParticles(particlesTex);

            canvas = ASXMLCanvas.LoadFromXML("ui/ExampleUI.xml");

            return SDL_AppResult.SDL_APP_CONTINUE;
        };
        static SDL_AppIterate_func SDL_AppIterate = (nint appstate) => {
            Input.SetCurrentState();

            Time.Update();
            Input.Update();

            freecam.Move();

            ParticleManager.UpdateParticles();
            //shrekT.position = Vector3.UnitY + Vector3.UnitZ * (float)MathHelper.Sin(Time.time);
            //shrekT.Rotate(Vector3.UnitY, 120 * Time.deltaTime);

            //cube!.transform.position = new Vector3(0.0f, (float)MathHelper.Sin(Time.time) + 0.5f, 0.0f);
            //cube.transform.Rotate(cube.transform.right + Vector3.UnitY, 30 * Time.deltaTime);

            RenderPipeline.Render();

            //Input.SetLastState();
            return SDL_AppResult.SDL_APP_CONTINUE;
        };
        unsafe static SDL_AppEvent_func SDL_AppEvent = (nint appstate, SDL_Event* sdlEvent) => {
            Input.OnEvent();
            RenderPipeline.OnEvent();

            return SDL_AppResult.SDL_APP_CONTINUE;
        };
        static SDL_AppQuit_func SDL_AppQuit = (nint appstate, SDL_AppResult result) => {
            RenderPipeline.Dispose();

            SDL_Quit();
        };
    }
}