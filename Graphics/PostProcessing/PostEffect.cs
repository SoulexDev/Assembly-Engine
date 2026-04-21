using OpenTK.Graphics.OpenGL4;

namespace AssemblyEngine.Graphics
{
    public class PostEffect
    {
        private Shader shader;
        public PostEffect(Shader shader)
        {
            this.shader = shader;
        }
        public void Render(uint texture)
        {
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            shader.SetTexture("uScreenTexture", 0);
            GL.BindVertexArray(FullscreenQuadMesh.mesh.vao);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }
}
