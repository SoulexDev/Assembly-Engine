using static SDL3.SDL;

namespace ASE
{
    public class Time
    {
        private static float lastFrameTime;
        public static float time = 0.0f;
        public static float deltaTime = 0.0f;
        public static float fixedDeltaTime = 0.0f;

        public static void Update()
        {
            time = SDL_GetTicks() / 1000.0f;
            deltaTime = time - lastFrameTime;
            lastFrameTime = time;
        }
    }
}
