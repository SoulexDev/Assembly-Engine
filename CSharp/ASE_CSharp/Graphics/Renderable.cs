using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ASE.Graphics
{
    public struct Renderable
    {
        private Transform transform;
        public Mesh mesh;
        public Material material;
        public Matrix4 modelMatrix;

        public Renderable(Transform transform, Mesh mesh)
        {
            this.transform = transform;
            this.mesh = mesh;

            RenderPipeline.AddRenderable(this);
        }
        public Renderable(Transform transform, Mesh mesh, Material material)
        {
            this.transform = transform;
            this.mesh = mesh;
            this.material = material;

            RenderPipeline.AddRenderable(this);
        }
        public void Draw(Camera camera)
        {
            if (mesh.count == 0 || material == null)
                return;

            Matrix4.CreateFromQuaternion(transform.rotation, out modelMatrix);
            modelMatrix *= Matrix4.CreateTranslation(transform.position);
            modelMatrix *= Matrix4.CreateScale(transform.scale);

            material.Use();

            material.shader.SetMatrix4("uModel", modelMatrix);
            material.shader.SetMatrix4("uView", camera.viewMatrix);
            material.shader.SetMatrix4("uProjection", camera.projectionMatrix);
            material.shader.SetMatrix4("uLightProjection", RenderPipeline.sunCam.projectionMatrix);
            material.shader.SetMatrix4("uLightView", RenderPipeline.sunCam.viewMatrix);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, RenderPipeline.shadowTex);
            material.shader.SetTexture("uShadowTex0", 0);

            material.shader.SetVector("uLightPos", RenderPipeline.sunCam.transform.position);
            material.shader.SetVector("uViewPos", camera.transform.position);
            material.shader.SetFloat("uTime", Time.time);

            mesh.Draw();
        }
        public void Draw(Camera camera, Shader shader)
        {
            if (mesh.count == 0)
                return;

            Matrix4.CreateFromQuaternion(transform.rotation, out modelMatrix);
            modelMatrix *= Matrix4.CreateTranslation(transform.position);
            modelMatrix *= Matrix4.CreateScale(transform.scale);

            shader.Use();

            shader.SetMatrix4("uModel", modelMatrix);
            shader.SetMatrix4("uView", camera.viewMatrix);
            shader.SetMatrix4("uProjection", camera.projectionMatrix);
            //shader.SetMatrix4("uLightSpace", RenderPipeline.sunCam.projectionMatrix * RenderPipeline.sunCam.viewMatrix);

            //shader.SetVector("uLightPos", RenderPipeline.sunCam.transform.position);
            //shader.SetVector("uViewPos", camera.transform.position);
            shader.SetFloat("uTime", Time.time);

            mesh.Draw();
        }
    }
}
