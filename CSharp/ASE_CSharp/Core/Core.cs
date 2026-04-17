using ASE.Graphics;
using SDL3;
using static SDL3.SDL;

//using var sdl = new Sdl(static builder => builder.SetAppName("Assembly Engine").InitializeSubSystems(SubSystems.Video | SubSystems.Audio));

//return sdl.Run(new Core(), args);

namespace ASE
{
    public class Core
    {
        public static string AssetsPath;
        public static void Main(string[] args)
        {
            AssetsPath = AppDomain.CurrentDomain.BaseDirectory;
            AssetsPath = Path.Combine(AssetsPath, "assets");
            Console.WriteLine(AssetsPath);
            SDL_EnterAppMainCallbacks(0, 0, SDL_AppInit, SDL_AppIterate, SDL_AppEvent, SDL_AppQuit);
        }
        static SDL_AppInit_func SDL_AppInit = (nint appState, int argc, nint argv) => {
            if (!RenderPipeline.Init())
            {
                return SDL_AppResult.SDL_APP_FAILURE;
            }
            Input.Init();

            return SDL_AppResult.SDL_APP_CONTINUE;
        };
        static SDL_AppIterate_func SDL_AppIterate = (nint appstate) => {
            Input.SetCurrentState();

            Time.Update();
            Input.Update();

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