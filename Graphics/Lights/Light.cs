using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    public enum LightType { Directional, Point, Spot, Area }
    public sealed class Light : Component
    {
        public LightType lightType;
        
        public Camera lightCamera;
        public RenderTexture shadowTex;

        public override void Init()
        {
            shadowTex = new RenderTexture(1024, 1024,
                RenderTextureType.Depth, TextureWrapMode.ClampToBorder,
                TextureMinFilter.Linear, TextureMagFilter.Linear);

            lightCamera = engineObject.AddComponent<Camera>("light camera").InitializeParameters(50, 50, 1, 100);

            lightType = LightType.Directional;

            RenderPipeline.lights.Add(this);
        }
        public override void OnDestroy()
        {
            RenderPipeline.lights.Remove(this);
        }
        public Light InitializeParameters(int shadowResolution)
        {
            shadowTex = new RenderTexture(
                shadowResolution, shadowResolution,
                RenderTextureType.Depth, TextureWrapMode.ClampToBorder,
                TextureMinFilter.Linear, TextureMagFilter.Linear);

            lightType = LightType.Directional;

            return this;
        }
        public void DrawShadows(List<ModelRenderer> modelRenderers)
        {
            switch (lightType)
            {
                case LightType.Directional:
                    DrawDirectionalShadow(modelRenderers);
                    break;
                case LightType.Point:
                    break;
                case LightType.Spot:
                    break;
                case LightType.Area:
                    break;
                default:
                    break;
            }
        }
        private void DrawDirectionalShadow(List<ModelRenderer> modelRenderers)
        {
            Vector3 desiredPosition = Camera.main.transform.position.Floor() - transform.forward * 50;

            lightCamera.transform.position = desiredPosition;
            lightCamera.transform.rotation = transform.rotation;

            //draw
            GL.Viewport(0, 0, shadowTex.width, shadowTex.height);

            shadowTex.Bind();

            GL.Clear(ClearBufferMask.DepthBufferBit);
            lightCamera.SetMatrices();

            GL.Disable(EnableCap.CullFace);
            foreach (ModelRenderer modelRenderer in modelRenderers)
            {
                modelRenderer.Draw(lightCamera, RenderPipeline.shadowShader);
            }
            GL.Enable(EnableCap.CullFace);

            shadowTex.Unbind();
        }
    }
}
