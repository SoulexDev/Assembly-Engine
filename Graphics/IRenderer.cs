namespace AssemblyEngine.Graphics
{
    public interface IRenderer
    {
        public void Draw(Camera camera);
        public void Draw(Camera camera, Shader shader);
    }
}