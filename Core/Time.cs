using System.Diagnostics;
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

        private static Stopwatch fixedFrameTimer;
        private static long fixedTimeOffset;
        public static void Init()
        {
            fixedFrameTimer = new Stopwatch();
            fixedFrameTimer.Start();
        }
        public static void Update()
        {
            time = SDL_GetTicks() / 1000.0f;
            deltaTime = time - lastFrameTime;
            lastFrameTime = time;
            
            if (fixedFrameTimer.ElapsedMilliseconds + fixedTimeOffset >= 20)
            {
                fixedTimeOffset = fixedFrameTimer.ElapsedMilliseconds + fixedTimeOffset - 20;
                fixedFrameTimer.Restart();

                fixedDeltaTime = time - lastFixedFrameTime;
                lastFixedFrameTime = time;
            }
        }
    }
}