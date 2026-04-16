using ASE.Graphics;
using ASE_CSharp;
using SDL3;
using static SDL3.SDL;

//using var sdl = new Sdl(static builder => builder.SetAppName("Assembly Engine").InitializeSubSystems(SubSystems.Video | SubSystems.Audio));

//return sdl.Run(new Core(), args);

namespace ASE
{
    public class Core
    {
        public static void Main(string[] args)
        {
            SDL.SDL_EnterAppMainCallbacks(0, 0, SDL_AppInit, SDL_AppIterate, SDL_AppEvent, SDL_AppQuit);
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
            return SDL_AppResult.SDL_APP_CONTINUE;
        };
        static SDL_AppQuit_func SDL_AppQuit = (nint appstate, SDL_AppResult result) => {
            SDL_Quit();
        };
    }
}