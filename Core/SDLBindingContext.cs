using OpenTK;
using static SDL3.SDL;

namespace AssemblyEngine
{
    internal class SDLBindingContext : IBindingsContext
    {
        public nint GetProcAddress(string procName)
        {
            return SDL_GL_GetProcAddress(procName);
        }
    }
}
