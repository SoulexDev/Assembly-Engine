using ASE.Graphics;
using ASE.Graphics.Testing;
using ASE.Testing;
using OpenTK.Mathematics;
using static SDL3.SDL;

//using var sdl = new Sdl(static builder => builder.SetAppName("Assembly Engine").InitializeSubSystems(SubSystems.Video | SubSystems.Audio));

//return sdl.Run(new Core(), args);

namespace ASE
{
    public class Core
    {
        public static string AssetsPath;
        private static Cube cube;
        private static FreeCam freecam;
        public static void Main(string[] args)
        {
            AssetsPath = AppDomain.CurrentDomain.BaseDirectory;
            AssetsPath = Path.Combine(AssetsPath, "resources");
            Console.WriteLine(AssetsPath);
            SDL_EnterAppMainCallbacks(0, 0, SDL_AppInit, SDL_AppIterate, SDL_AppEvent, SDL_AppQuit);
        }
        static SDL_AppInit_func SDL_AppInit = (nint appState, int argc, nint argv) => {
            if (!RenderPipeline.Init())
            {
                return SDL_AppResult.SDL_APP_FAILURE;
            }
            Input.Init();

            freecam = new FreeCam(Camera.main);

            ResourceLoader.LoadResource(out Texture2D planeTex, "textures/tex_atlas.png");
            ResourceLoader.LoadResource(out Texture2D cubeTex, "textures/tex_atlas.png");
            Plane plane = PlaneGenerator.Generate(planeTex, 10, 10);
            plane.transform.scale = Vector3.One * 4.0f;

            cube = new Cube(cubeTex);

            return SDL_AppResult.SDL_APP_CONTINUE;
        };
        static SDL_AppIterate_func SDL_AppIterate = (nint appstate) => {
            Input.SetCurrentState();

            Time.Update();
            Input.Update();

            freecam.Move();

            cube!.transform.position = new Vector3(0.0f, (float)MathHelper.Sin(Time.time) + 0.5f, 0.0f);
            cube.transform.Rotate(cube.transform.right + Vector3.UnitY, 30 * Time.deltaTime);

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