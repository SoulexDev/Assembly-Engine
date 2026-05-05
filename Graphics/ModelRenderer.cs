using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    public sealed class ModelRenderer : Component
    {
        public bool isDirty = true;

        private Model model;
        public Matrix4 modelMatrix;

        public ModelRenderer()
        {

        }
        public ModelRenderer(Model model, Matrix4 modelMatrix)
        {
            this.model = model;
            this.modelMatrix = modelMatrix;

            isDirty = false;

            RenderPipeline.AddModelRenderer(this);
        }
        //~ModelRenderer()
        //{
        //    RenderPipeline.RemoveModelRenderer(this);
        //}
        public override void Init()
        {
            RenderPipeline.AddModelRenderer(this);
        }
        public override void OnDestroy()
        {
            RenderPipeline.RemoveModelRenderer(this);
        }
        public void SetMaterial(Material mat, int meshIndex = 0)
        {
            model.meshes[0] = (model.meshes[0].Item1, mat);
        }
        public void SetModel(Model model)
        {
            this.model = model;
        }
        public void Draw(Camera camera)
        {
            if (model != null && model.meshes.Count == 0)
                return;

            isDirty = true;
            if (isDirty)
            {
                if (transform != null)
                {
                    Matrix4.CreateScale(transform.scale, out modelMatrix);
                    modelMatrix *= Matrix4.CreateFromQuaternion(transform.rotation);
                    modelMatrix *= Matrix4.CreateTranslation(transform.position);
                }

                isDirty = false;
            }

            foreach (var mesh in model.meshes)
            {
                //use material
                Material mat = mesh.Item2;

                if (mat == null)
                    continue;

                mat.Use();

                mat.shader.SetMatrix4("uModel", modelMatrix);
                mat.shader.SetMatrix4("uView", camera.viewMatrix);
                mat.shader.SetMatrix4("uProjection", camera.projectionMatrix);

                for (int i = 0; i < RenderPipeline.lights.Count; i++)
                {
                    Light light = RenderPipeline.lights[i];

                    mat.shader.SetMatrix4("uLightProjection", light.lightCamera.projectionMatrix);
                    mat.shader.SetMatrix4("uLightView", light.lightCamera.viewMatrix);

                    if (light.lightType == LightType.Directional)
                        mat.shader.SetVector($"uLightPos{i}", new Vector4(light.transform.forward, 1));
                    else
                        mat.shader.SetVector($"uLightPos{i}", new Vector4(light.transform.position, 0));

                    GL.ActiveTexture(TextureUnit.Texture0 + i);
                    GL.BindTexture(TextureTarget.Texture2D, light.shadowTex);

                    mat.shader.SetTexture($"uShadowTex{i}", 0);

                    //GL.BindTexture(TextureTarget.Texture2D, 0);
                }

                mat.shader.SetVector("uViewPos", camera.transform.position);
                mat.shader.SetFloat("uTime", Time.time);

                mesh.Item1.Draw();
            }
        }
        public void Draw(Camera camera, Shader shader)
        {
            if (model != null && model.meshes.Count == 0)
                return;

            isDirty = true;
            if (isDirty)
            {
                if (transform != null)
                {
                    Matrix4.CreateScale(transform.scale, out modelMatrix);
                    modelMatrix *= Matrix4.CreateFromQuaternion(transform.rotation);
                    modelMatrix *= Matrix4.CreateTranslation(transform.position);
                }

                isDirty = false;
            }

            shader.Use();

            shader.SetMatrix4("uModel", modelMatrix);
            shader.SetMatrix4("uView", camera.viewMatrix);
            shader.SetMatrix4("uProjection", camera.projectionMatrix);
            //shader.SetMatrix4("uLightSpace", RenderPipeline.sunCam.projectionMatrix * RenderPipeline.sunCam.viewMatrix);

            //shader.SetVector("uLightPos", RenderPipeline.sunCam.transform.position);
            //shader.SetVector("uViewPos", camera.transform.position);
            shader.SetFloat("uTime", Time.time);

            foreach (var mesh in model.meshes)
            {
                mesh.Item1.Draw();
            }
        }
    }
}
