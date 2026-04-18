using ASE.Graphics;
using static SDL3.SDL;

namespace ASE
{
    public class Input
    {
        //private ref struct InputBuffer
        //{
        //    public Span<SDLBool> keyStates;
        //}
        //private static InputBuffer currentKeyStates;
        //private static InputBuffer lastKeyStates;

        private static float lastMousePosX;
        private static float lastMousePosY;

        public static float horizontal;
        public static float vertical;
        public static float longitudinal;
        public static float mouseDeltaX;
        public static float mouseDeltaY;

        public static void Init()
        {
            //SDL_SetWindowRelativeMouseMode(RenderPipeline.window, true);
            //SDL_SetWindowMouseGrab(RenderPipeline.window, true);
            SDL_CaptureMouse(true);

            lastMousePosX = Engine.screenWidth * 0.5f;
            lastMousePosX = Engine.screenHeight * 0.5f;

            //InputBuffer i = new InputBuffer() { keyStates = SDL_GetKeyboardState() };
        }
        public static void Update()
        {
            float hl = IsKeyPressed(SDL_Scancode.SDL_SCANCODE_A) ? 1.0f : 0.0f;
            float hr = IsKeyPressed(SDL_Scancode.SDL_SCANCODE_D) ? -1.0f : 0.0f;
            horizontal = hl + hr;

            float vf = IsKeyPressed(SDL_Scancode.SDL_SCANCODE_W) ? 1.0f : 0.0f;
            float vb = IsKeyPressed(SDL_Scancode.SDL_SCANCODE_S) ? -1.0f : 0.0f;
            vertical = vf + vb;

            float lu = IsKeyPressed(SDL_Scancode.SDL_SCANCODE_E) ? 1.0f : 0.0f;
            float ld = IsKeyPressed(SDL_Scancode.SDL_SCANCODE_Q) ? -1.0f : 0.0f;
            longitudinal = lu + ld;

            SDL_MouseButtonFlags buttonFlags = SDL_GetMouseState(out float x, out float y);

            mouseDeltaX = (lastMousePosX - x) / Engine.screenWidth;
            mouseDeltaY = (lastMousePosY - y) / Engine.screenHeight;

            lastMousePosX = x;
            lastMousePosY = y;
        }
        public static void OnEvent()
        {

        }
        public static void SetCurrentState()
        {

        }
        public static void GetCurrentState()
        {

        }
        public static bool GetKeyDown(SDL_Scancode key)
        {
            return SDL_GetKeyboardState()[(int)key];
        }
        public static bool GetKeyUp(SDL_Scancode key)
        {
            return !SDL_GetKeyboardState()[(int)key];
        }
        public static bool IsKeyPressed(SDL_Scancode key)
        {
            return SDL_GetKeyboardState()[(int)key];
        }
    }
}
