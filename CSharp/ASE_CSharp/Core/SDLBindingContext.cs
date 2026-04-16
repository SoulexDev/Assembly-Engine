using OpenTK;
using SDL3;
using static SDL3.SDL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASE
{
    internal class SDLBindingContext : IBindingsContext
    {
        public nint GetProcAddress(string procName)
        {
            return SDL_GL_GetProcAddress(procName);
        }
    }
}
