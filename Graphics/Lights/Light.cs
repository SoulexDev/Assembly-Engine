using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Numerics;

namespace AssemblyEngine.Graphics
{
    public enum LightType { Directional, Point, Spot, Area }
    public class Light
    {
        public Transform transform;
        public LightType lightType;
        
        public Camera lightCamera;
        public RenderTexture shadowTex;

        public Light(int shadowResolution)
        {
            transform = new Transform();

            shadowTex = new RenderTexture(
                shadowResolution, shadowResolution, 
                RenderTextureType.Depth, TextureWrapMode.ClampToBorder, 
                TextureMinFilter.Linear, TextureMagFilter.Linear);

            lightCamera = new Camera(50, 50, 1f, 100.0f);

            lightType = LightType.Directional;
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
            lightCamera.transform.position = Camera.main.transform.position - transform.forward * 50;
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
