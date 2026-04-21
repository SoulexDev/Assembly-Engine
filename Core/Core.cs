using AssemblyEngine.ECS;
using AssemblyEngine.Graphics;
using AssemblyEngine.Graphics.Testing;
using AssemblyEngine.Testing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static SDL3.SDL;

//using var sdl = new Sdl(static builder => builder.SetAppName("Assembly Engine").InitializeSubSystems(SubSystems.Video | SubSystems.Audio));

//return sdl.Run(new Core(), args);

namespace AssemblyEngine
{
    public class Core
    {
        public static string AssetsPath;
        private static Cube cube;
        private static FreeCam freecam;

        public static Shader defaultShader;
        private static Transform shrekT;

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

            //load stuff
            //ResourceLoader.LoadResource(out Renderable renderable, "models/guitartypeshi.fbx");
            ResourceLoader.LoadResource(out Renderable shrekRenderable, "models/shrek/Shrek.obj");
            ResourceLoader.LoadResource(out Renderable donkeyRenderable, "models/donkey/Donkey.obj");
            ResourceLoader.LoadResource(out Texture2D particlesTex, "textures/gaussian_circle_1.png");

            Material shrekMat = new Material(defaultShader);
            Material donkeyMat = new Material(defaultShader);

            if (ResourceLoader.LoadResource(out Texture2D shrekAlbedo, "models/shrek/t_shrek_c.png"))
            {
                shrekMat.texture2Ds.Add(("uMainTex", shrekAlbedo));
            }
            if (ResourceLoader.LoadResource(out Texture2D donkeyAlbedo, "models/donkey/t_Donkey_c.png"))
            {
                donkeyMat.texture2Ds.Add(("uMainTex", donkeyAlbedo));
            }

            shrekT = shrekRenderable.transform;

            shrekRenderable.SetMaterial(shrekMat);
            donkeyRenderable.SetMaterial(donkeyMat);

            shrekRenderable.transform.position = Vector3.UnitX * 0.5f + Vector3.UnitY;
            donkeyRenderable.transform.position = -Vector3.UnitX * 0.5f + Vector3.UnitY;

            donkeyRenderable.transform.SetParent(shrekRenderable.transform, true);

            shrekRenderable.transform.scale = Vector3.One * 0.25f;
            donkeyRenderable.transform.scale = Vector3.One * 0.5f * 0.25f;

            //ResourceLoader.LoadResource(out Texture2D guitarTex, "models/1001_albedo.jpg");
            //mat.texture2Ds.Add(("uMainTex", guitarTex));
            //renderable.SetMaterial(mat);

            //renderable.transform.Rotate(Vector3.UnitX, -90);

            freecam = new FreeCam(Camera.main);

            //ResourceLoader.LoadResource(out Texture2D planeTex, "textures/tex_atlas.png");
            ResourceLoader.LoadResource(out Texture2D tex, "textures/tex_atlas.png");
            Plane plane = PlaneGenerator.Generate(tex, PrimitiveType.Triangles, 10, 10);
            plane.transform.position = Vector3.UnitY;

            //ParticleManager.EmitParticles(particlesTex);

            //cube = new Cube(tex);

            return SDL_AppResult.SDL_APP_CONTINUE;
        };
        static SDL_AppIterate_func SDL_AppIterate = (nint appstate) => {
            Input.SetCurrentState();

            Time.Update();
            Input.Update();

            freecam.Move();

            //shrekT.position = Vector3.UnitY + Vector3.UnitZ * (float)MathHelper.Sin(Time.time);
            shrekT.Rotate(Vector3.UnitY, 120 * Time.deltaTime);

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