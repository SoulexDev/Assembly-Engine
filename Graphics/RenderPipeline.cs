using static SDL3.SDL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using AssemblyEngine.UI;
using AssemblyEngine.GUI;

namespace AssemblyEngine.Graphics
{
    public class RenderPipeline
    {
        public static nint window;
        public static nint glContext;

        public static RenderTexture shadowTex;
        public static Shader shadowShader;
        public static Shader skyboxShader;
        public static Camera sunCam;

        private static (RenderTexture, RenderTexture) postProcessBuffers;

        //soon, a new architecture will be implemented that should massively improve performance for large scenes.
        //the renderpipeline will hold a list of materials, each corresponding to a list of "renderable indices".
        //renderable indices are simple a pair of a renderable, and a material index. since we want to distinguish objects as a whole
        //from one another, it's necessary that renderables are aware of their mesh parts. sorting meshes exclusively by material
        //would jumble up renderables. a renderable would have meshes that are in multiple lists, which isn't ideal, and developing a system
        //to track meshes would be prone to bugs and would be difficult to do properly.
        //meshes are drawn by the renderable, from the renderpipeline material list

        //private static Dictionary<Material, List<ModelRenderIndex>> materialModelLists = new Dictionary<Material, List<ModelRenderIndex>>();

        private static List<ModelRenderer> modelRenderers = new List<ModelRenderer>();
        private static List<ASXMLCanvas> canvases = new List<ASXMLCanvas>();
        private static List<PostEffect> postEffects = new List<PostEffect>();
        internal static List<Light> lights = new List<Light>();

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
                Console.WriteLine("Could not set v-sync");
                return false;
            }

            FullscreenQuadMesh.Create();
            SkyboxMesh.Create();
            QuadMesh.Create();

            EngineObject sun = EngineObjectFactory.Instantiate("Sun");
            //TODO: special internal stuff for rendering to allow users to interface with the rendering system in a managable way
            //users shouldn't have to set the light resolution everytime. it should be global. they should still be able to change the resolution per light, though
            //there should be camera components and interal camera. camera components interface with internal cameras
            sun.AddComponent<Light>("directional light").InitializeParameters(2048);

            postProcessBuffers = (
                new RenderTexture(Engine.screenWidth, Engine.screenHeight, RenderTextureType.Normal),
                new RenderTexture(Engine.screenWidth, Engine.screenHeight, RenderTextureType.Normal));

            ResourceLoader.LoadResource(out Shader gammaShader, 
                ("internal/shaders/post_effect", ShaderType.VertexShader), 
                ("internal/shaders/gamma_correction_effect", ShaderType.FragmentShader));

            PushPostEffect(new PostEffect(gammaShader));
            
            ResourceLoader.LoadResource(out shadowShader, 
                ("internal/shaders/shadow", ShaderType.VertexShader), 
                ("internal/shaders/shadow", ShaderType.FragmentShader));

            ResourceLoader.LoadResource(out skyboxShader,
                ("internal/shaders/skybox/skybox", ShaderType.VertexShader),
                ("internal/shaders/skybox/skybox_procedural", ShaderType.FragmentShader));

            Camera.main = new Camera();
            //Camera.main = sunCam;

            return true;
        }
        public static void AddModelRenderer(ModelRenderer modelRenderer)
        {
            modelRenderers.Add(modelRenderer);
        }
        public static void RemoveModelRenderer(ModelRenderer modelRenderer)
        {
            modelRenderers.Remove(modelRenderer);
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

            //render light depth for shadows
            foreach (var light in lights)
            {
                //light.transform.Rotate(Vector3.UnitZ, 240 * Time.deltaTime);
                light.transform.rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 50) * 
                    Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegToRad * 45);

                light.DrawShadows(modelRenderers);
            }

            //render scene
            GL.Viewport(0, 0, Engine.screenWidth, Engine.screenHeight);

            if (postEffects.Count > 0)
                postProcessBuffers.Item1.Bind();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Camera.main.SetMatrices();
            foreach (ModelRenderer modelRenderer in modelRenderers)
            {
                modelRenderer.Draw(Camera.main);
            }

            //render particles
            //ParticleManager.DrawParticles();

            //render skybox
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);

            skyboxShader.Use();
            skyboxShader.SetMatrix4("uProjection", Camera.main.projectionMatrix);
            skyboxShader.SetMatrix4("uView", Camera.main.viewMatrix.ClearTranslation());
            SkyboxMesh.mesh.Draw();

            //disable depth testing for rendering ui and post effects
            GL.Disable(EnableCap.DepthTest);

            //render post effects
            if (postEffects.Count > 0)
            {
                postProcessBuffers.Item1.Unbind();
                PostProcess();
            }

            //render ui
            foreach (var canvas in canvases)
            {
                canvas.Draw();
            }

            GUIManager.Render();

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
