using ASE.Physics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ASE.Graphics
{
    public class Renderable
    {
        private Transform transform;
        public List<(Mesh, Material)> meshes = new List<(Mesh, Material)>();
        public Matrix4 modelMatrix;

        public AABB boundingBox;

        public Renderable(Transform transform, params (Mesh, Material)[] meshes)
        {
            this.transform = transform;
            this.meshes.AddRange(meshes);

            RenderPipeline.AddRenderable(this);
        }
        public void Draw(Camera camera)
        {
            if (meshes.Count == 0)
                return;

            Matrix4.CreateFromQuaternion(transform.rotation, out modelMatrix);
            modelMatrix *= Matrix4.CreateTranslation(transform.position);
            modelMatrix *= Matrix4.CreateScale(transform.scale);

            foreach (var mesh in meshes)
            {
                //use material
                Material mat = mesh.Item2;

                if (mat == null)
                    continue;

                mat.Use();

                mat.shader.SetMatrix4("uModel", modelMatrix);
                mat.shader.SetMatrix4("uView", camera.viewMatrix);
                mat.shader.SetMatrix4("uProjection", camera.projectionMatrix);
                mat.shader.SetMatrix4("uLightProjection", RenderPipeline.sunCam.projectionMatrix);
                mat.shader.SetMatrix4("uLightView", RenderPipeline.sunCam.viewMatrix);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, RenderPipeline.shadowTex);
                mat.shader.SetTexture("uShadowTex0", 0);

                mat.shader.SetVector("uLightPos", RenderPipeline.sunCam.transform.position);
                mat.shader.SetVector("uViewPos", camera.transform.position);
                mat.shader.SetFloat("uTime", Time.time);

                mesh.Item1.Draw();
            }
        }
        public void Draw(Camera camera, Shader shader)
        {
            if (meshes.Count == 0)
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

            foreach (var mesh in meshes)
            {
                mesh.Item1.Draw();
            }
        }
    }
}
