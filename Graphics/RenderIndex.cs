namespace AssemblyEngine.Graphics
{
    internal class RenderIndex
    {
        public IRenderer renderer;
        public int materialMeshIndex;

        public RenderIndex(IRenderer renderer, int materialMeshIndex)
        {
            this.renderer = renderer;
            this.materialMeshIndex = materialMeshIndex;
        }
    }
}