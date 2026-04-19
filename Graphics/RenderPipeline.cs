using static SDL3.SDL;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace ASE.Graphics
{
    public class RenderPipeline
    {
        //private Window window = default!;
        public static nint window;
        public static nint glContext;

        public static RenderTexture shadowTex;
        public static Shader shadowShader;
        public static Camera sunCam;

        private static (RenderTexture, RenderTexture) postProcessBuffers;

        private static List<Renderable> renderables = new List<Renderable>();
        private static List<PostEffect> postEffects = new List<PostEffect>();

        public static bool Init()
        {
            if (!SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO | SDL_InitFlags.SDL_INIT_AUDIO))
            {
                return false;
            }

            SDL_GL_LoadLibrary("");

            window = SDL_CreateWindow("Assembly Engine", Engine.screenWidth, Engine.screenHeight, SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

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

            FullscreenQuadMesh.Create();

            shadowTex = new RenderTexture(2048, 2048, RenderTextureType.Depth, TextureWrapMode.ClampToBorder, TextureMinFilter.Linear, TextureMagFilter.Linear);
            sunCam = new Camera(40.0f, 40.0f, 1.0f, 100.0f);
            sunCam.transform.position = new Vector3(5.0f, 10.0f, 5.0f) * 4.0f;
            sunCam.transform.rotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegToRad * -135.0f);
            sunCam.transform.rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegToRad * 45.0f);

            postProcessBuffers = (
                new RenderTexture(Engine.screenWidth, Engine.screenHeight, RenderTextureType.Normal),
                new RenderTexture(Engine.screenWidth, Engine.screenHeight, RenderTextureType.Normal));

            ResourceLoader.LoadResource(out Shader gammaShader, 
                ("shaders/internal/post_effect", ShaderType.VertexShader), 
                ("shaders/internal/gamma_correction_effect", ShaderType.FragmentShader));

            //PushPostEffect(new PostEffect(gammaShader));

            ResourceLoader.LoadResource(out shadowShader, 
                ("shaders/internal/shadow", ShaderType.VertexShader), 
                ("shaders/internal/shadow", ShaderType.FragmentShader));

            Camera.main = new Camera();
            //Camera.main = sunCam;

            return true;
        }
        public static void AddRenderable(Renderable renderable)
        {
            renderables.Add(renderable);
        }
        public static void RemoveRenderable(Renderable renderable)
        {
            renderables.Remove(renderable);
        }
        public static void PushPostEffect(PostEffect postEffect)
        {
            postEffects.Add(postEffect);
        }
        public static void PopPostEffect()
        {
            postEffects.RemoveAt(postEffects.Count - 1);
        }
        public static void ClearPostEffects()
        {
            postEffects.Clear();
        }
        public static void Render()
        {
            //enable depth testing
            GL.Enable(EnableCap.DepthTest);

            //render sun depth for shadows
            GL.Viewport(0, 0, shadowTex.width, shadowTex.height);
            //sunCam.transform.position = Camera.main.transform.position + new Vector3(5.0f, 5.0f, 5.0f) * 4.0f;

            shadowTex.Bind();

            GL.Clear(ClearBufferMask.DepthBufferBit);
            sunCam.SetMatrices();

            GL.Disable(EnableCap.CullFace);
            foreach (Renderable renderable in renderables)
            {
                renderable.Draw(sunCam, shadowShader);
            }
            GL.Enable(EnableCap.CullFace);

            shadowTex.Unbind();

            //render scene
            GL.Viewport(0, 0, Engine.screenWidth, Engine.screenHeight);

            if (postEffects.Count > 0)
                postProcessBuffers.Item1.Bind();

            GL.ClearColor(0.4f, 0.5f, 0.7f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Camera.main.SetMatrices();
            foreach (Renderable renderable in renderables)
            {
                renderable.Draw(Camera.main);
            }

            if (postEffects.Count > 0)
                postProcessBuffers.Item1.Unbind();

            //disable depth testing for post processing
            GL.Disable(EnableCap.DepthTest);

            if (postEffects.Count > 0)
                PostProcess();

            SDL_GL_SwapWindow(window);
        }
        private static void SwapPostBuffers()
        {
            (postProcessBuffers.Item1, postProcessBuffers.Item2) = (postProcessBuffers.Item2, postProcessBuffers.Item1);
        }
        public static void PostProcess()
        {
            SwapPostBuffers();

            bool isOutput = false;
            for (int i = 0; i < postEffects.Count; i++)
            {
                isOutput = i == postEffects.Count - 1;

                if (!isOutput)
                    postProcessBuffers.Item1.Bind();

                postEffects[i].Render(postProcessBuffers.Item2);

                if (!isOutput)
                    postProcessBuffers.Item1.Unbind();

                SwapPostBuffers();
            }
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
