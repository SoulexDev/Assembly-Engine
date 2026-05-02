using static SDL3.SDL;

namespace AssemblyEngine
{
    public class Time
    {
        private static float lastFrameTime;
        private static float lastFixedFrameTime;
        public static float time = 0.0f;
        public static float deltaTime = 0.0f;
        public static float fixedDeltaTime = 0.0f;

        //public static void Init()
        //{
            
        //}
        internal static void Update()
        {
            time = SDL_GetTicks() / 1000.0f;
            deltaTime = time - lastFrameTime;
            lastFrameTime = time;
        }
        internal static void FixedUpdate()
        {
            fixedDeltaTime = time - lastFixedFrameTime;
            lastFixedFrameTime = time;
        }
    }
}