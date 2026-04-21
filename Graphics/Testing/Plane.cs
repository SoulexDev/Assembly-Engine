namespace AssemblyEngine.Graphics.Testing
{
    internal class Plane
    {
        public Transform transform;
        public Renderable renderable;

        public Plane(Mesh mesh, Material mat)
        {
            transform = new Transform();
            renderable = new Renderable(transform, (mesh, mat));
        }
    }
}
