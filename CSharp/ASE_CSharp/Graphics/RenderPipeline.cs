using SDL3;
using static SDL3.SDL;
using OpenTK.Graphics.OpenGL4;

namespace ASE.Graphics
{
    public class RenderPipeline
    {
        //private Window window = default!;
        public static nint window;
        public static nint glContext;

        public static bool Init()
        {
            if (!SDL_Init(SDL.SDL_InitFlags.SDL_INIT_VIDEO | SDL.SDL_InitFlags.SDL_INIT_AUDIO))
            {
                return false;
            }

            SDL_GL_LoadLibrary("");

            window = SDL_CreateWindow("Assembly Engine", Engine.width, Engine.height, SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MAJOR_VERSION, 4);
            SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MINOR_VERSION, 6);
            SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_CORE);

            glContext = SDL_GL_CreateContext(window);

            GL.LoadBindings(new SDLBindingContext());

            if (!SDL_GL_SetSwapInterval(1))
            {
                Console.WriteLine("Coould not set v-sync");
                return false;
            }

            return true;
        }
        public static void PushPostEffect()
        {

        }
        public static void PopPostEffect()
        {

        }
        public static void ClearPostEffects()
        {

        }
        public static void Render()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.4f, 0.5f, 0.7f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.DepthTest);

            SDL_GL_SwapWindow(window);
        }
        public static void OnEvent()
        {

        }
        public static void Dispose()
        {
            SDL_GL_UnloadLibrary();
        }
    }
}
